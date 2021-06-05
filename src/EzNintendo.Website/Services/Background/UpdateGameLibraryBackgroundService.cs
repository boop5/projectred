using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EzNintendo.Common.Utilities;
using EzNintendo.Data;
using EzNintendo.Data.Nintendo;
using EzNintendo.Domain.eShop;
using EzNintendo.Domain.Nintendo;
using EzNintendo.Website.Services.Data;
using EzNintendo.Website.Services.Nintendo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EzNintendo.Website.Services.Background
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Runtime creates Instance.")]
    public sealed class UpdateGameLibraryBackgroundService : LoopingBackgroundService
    {
        private readonly eShopApi _api;
        private readonly ApplicationDbContextFactory _ctxFactory;
        private readonly ThrowHelper _throw;

        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter", Justification = "ILogger needs to be generic.")]
        public UpdateGameLibraryBackgroundService(ILogger<UpdateGameLibraryBackgroundService> log,
                                                  eShopApi api,
                                                  ApplicationDbContextFactory ctxFactory,
                                                  ThrowHelper @throw) : base(log)
        {
            _api = api;
            _ctxFactory = ctxFactory;
            _throw = @throw;
        }

        protected override TimeSpan GetTaskInterval()
        {
            return TimeSpan.FromHours(4);
        }

        protected override TimeSpan GetInitialDelay()
        {
            return TimeSpan.FromHours(240);
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            Log.LogInformation("Update Game-Library");

            var ctx = _ctxFactory.Get();

            foreach (eShopRegion region in Enum.GetValues(typeof(eShopRegion)))
            {
                if (region != eShopRegion.Europe)
                {
                    continue;
                }
                // TODO: FIX FOR ALL REGIONS BRUH

                await UpdateLibrary(ctx, region, stoppingToken);
            }

            Log.LogDebug("Finished updating Game-Library");
        }

        private async Task UpdateLibrary(ApplicationDbContext ctx, eShopRegion region, CancellationToken stoppingToken = default)
        {
            _throw.IfNull(ctx, nameof(ctx));

            var localLibrary = await ctx.Games
                                        .Include(g => g.GameLanguages)
                                        .Include(g => g.GameCategories)
                                        .ToListAsync(stoppingToken);
            var officialLibrary = await _api.GetAllGames(region);

            foreach (var dto in officialLibrary)
            {
                try
                {
                    await using var transaction = await ctx.Database.BeginTransactionAsync(stoppingToken);
                    var game = AddOrUpdate(localLibrary.FirstOrDefault(local => local.Title == dto.Title), dto);
                    UpdateRegionIdentifier(game, dto, region);
                    UpdateControllers(game, dto);
                    UpdateCategories(ctx, game, dto);
                    UpdateLanguages(ctx, game, dto);

                    var entry = ctx.ChangeTracker
                                   .Entries()
                                   .SingleOrDefault(x => Equals(x.Entity, game));

                    if (entry == null)
                    {
                        await ctx.AddAsync(game, stoppingToken);
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        ctx.Update(game);
                    }

                    if (ctx.ChangeTracker.HasChanges())
                    {
                        await ctx.SaveChangesAsync(stoppingToken);
                        await transaction.CommitAsync(stoppingToken);
                    }

                    // await DownloadMissingCovers(game.GetNormalizedTitle(), dto); // todo: download covers bruh, own service
                }
                catch (Exception e)
                {
                    Debugger.Break();
                    Log.LogError(e, "Failed to Update Game");
                }
            }
        }

        private void UpdateControllers(Game game, GameDTO dto)
        {
            if (dto.CompatibleController != null)
            {
                game.ProController = dto.CompatibleController.Any(x => x == "nintendoswitch_pro_controller");
            }
        }

        private void UpdateRegionIdentifier(Game game, GameDTO dto, eShopRegion region)
        {
            if (dto.NsUid == null)
            {
                return;
            }

            switch (region)
            {
                case eShopRegion.Europe:
                    game.NsUid_EU = dto.NsUid;
                    break;
                case eShopRegion.Japan:
                    game.NsUid_JP = dto.NsUid;
                    break;
                case eShopRegion.UnitedStates:
                    game.NsUid_US = dto.NsUid;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(region), region, null);
            }
        }

        private void UpdateLanguages(ApplicationDbContext ctx, Game game, GameDTO dto)
        {
            if (dto.Languages == null)
            {
                return;
            }

            if (game.GameLanguages == null)
            {
                game.GameLanguages = new List<GameLanguage>();
            }

            // add new languages
            var newLanguages = dto.Languages
                                  .Except(game.GameLanguages.Select(g => g.Language))
                                  .ToList();

            if (newLanguages.Any())
            {
                Log.LogInformation("Add new Languages to {game} ({languages})", game.Title, string.Join(", ", newLanguages));

                game.GameLanguages = game.GameLanguages.Union(newLanguages.Select(language => new GameLanguage(game, language))).ToList();
            }

            // removed old languages
            var oldLanguages = game.GameLanguages.Where(language => !dto.Languages.Contains(language.Language)).ToList();

            if (oldLanguages.Any())
            {
                Log.LogInformation("Remove obsolete languages {language} from {game}", string.Join(",", oldLanguages), game.Title);

                oldLanguages.ForEach(x => game.GameLanguages.Remove(x)); // todo: do I really need to remove entities manually?
                ctx.RemoveRange(oldLanguages);
            }
        }

        private void UpdateCategories(ApplicationDbContext ctx, Game game, GameDTO dto)
        {
            if (dto.Categories == null)
            {
                return;
            }

            if (game.GameCategories == null)
            {
                game.GameCategories = new List<GameCategory>();
            }

            // add new categories
            var newCategories = dto.Categories
                                   .Except(game.GameCategories.Select(g => g.Category))
                                   .ToList();

            if (newCategories.Any())
            {
                Log.LogInformation("Add new categories to {game} ({categories})", game.Title, string.Join(", ", newCategories));

                game.GameCategories = game.GameCategories.Union(newCategories.Select(category => new GameCategory(game, category))).ToList();
            }

            // removed old categories
            var oldCategories = game.GameCategories.Where(category => !dto.Categories.Contains(category.Category)).ToList();

            if (oldCategories.Any())
            {
                Log.LogInformation("Remove obsolete categories {categories} from {game}", string.Join(",", oldCategories), game.Title);

                oldCategories.ForEach(x => game.GameCategories.Remove(x)); // todo: do I really need to remove entities manually?
                ctx.RemoveRange(oldCategories);
            }
        }

        private Game AddOrUpdate(Game localGame, GameDTO dto)
        {
            if (localGame == null)
            {
                localGame = AddNewGame(dto);
            }
            else if (!localGame.Equals(dto, out var reason)) // todo: fix comparison
            {
                UpdateExistingGame(dto, localGame, reason);
            }

            return localGame;
        }

        private void UpdateExistingGame(GameDTO dto, Game game, string reason)
        {
            _throw.IfNull(game, nameof(game))
                  .IfNull(dto, nameof(dto));

            Log.LogInformation("Update \"{game}\" because {reason}", dto.Title, reason);

            game.UpdateFrom(dto);
        }

        private Game AddNewGame(GameDTO dto)
        {
            _throw.IfNull(dto, nameof(dto));

            Log.LogInformation("Add new Game to the library! {game}", dto.Title);

            var game = Game.FromDTO(dto);

            if (!Game.Validate(game, out var reason))
            {
                Log.LogWarning("Failed to add new Game. Could not validate the Entity. {reason}", reason);
                throw new ValidationException();
            }

            return game;
        }
    }
}