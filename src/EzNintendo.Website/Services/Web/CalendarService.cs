using System;
using System.Linq;
using System.Threading.Tasks;
using EzNintendo.Data;
using EzNintendo.Data.Nintendo;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EzNintendo.Website.Services.Web
{
    public sealed class CalendarService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly CalendarSerializer _serializer = new CalendarSerializer();
        
        private DateTime _lastCache = DateTime.MinValue;
        private string _lastCalendar;

        public CalendarService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        
        /// <summary>
        ///     Gets an ICal Feed for Game Release-Dates.
        /// </summary>
        /// <param name="invalidateCache">
        ///     Generates a new Calendar if the cached one is older. Defaults to 6 hours.
        /// </param>
        /// <returns>
        ///     An ICal Feed for each Release-Dates.
        /// </returns>
        public async Task<string> GetGamesICal(TimeSpan invalidateCache = default)
        {
            if (invalidateCache == default)
            {
                invalidateCache = TimeSpan.FromHours(6);
            }

            var diff = DateTime.UtcNow - _lastCache;
            if (string.IsNullOrEmpty(_lastCalendar) || diff > invalidateCache)
            {
                var calendar = new Calendar();
                var serviceProvider = _scopeFactory.CreateScope().ServiceProvider;
                await using var dbContext = serviceProvider.GetService<ApplicationDbContext>();
                var games = await dbContext.Games
                                            .Where(g => g.ReleaseDate != default)
                                            .Select(g => new Game
                                            {
                                                ReleaseDate =  g.ReleaseDate,
                                                Title =  g.Title,
                                                Excerpt = g.Excerpt,
                                                Publisher = g.Publisher
                                            })
                                            .ToListAsync();

                games.ForEach(g => calendar.Events.Add(CreateEventFromGame(g)));

                _lastCalendar = _serializer.SerializeToString(calendar);
                _lastCache = DateTime.UtcNow;
            }

            return _lastCalendar;
        }

        private CalendarEvent CreateEventFromGame(Game game)
        {
            var @event = new CalendarEvent
            {
                Start = new CalDateTime((DateTime) game.ReleaseDate),
                End = new CalDateTime((DateTime) game.ReleaseDate),
                Summary = game.Title,
                IsAllDay = true,
                // Url = new Uri($"https://eznintendo.lsc.pw/game/{game.FsId}") // todo: pls fix fsid crap
            };

            if (!string.IsNullOrEmpty(game.Excerpt))
            {
                @event.Description = game.Excerpt;
            }

            if (!string.IsNullOrEmpty(game.Publisher))
            {
                @event.Organizer = new Organizer(game.Publisher);
            }

            return @event;
        }
    }
}