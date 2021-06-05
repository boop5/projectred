using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ChartJSCore.Helpers;
using ChartJSCore.Models;
using EzNintendo.Data;
using EzNintendo.Data.Nintendo;
using EzNintendo.Website.Models;
using EzNintendo.Website.Services.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EzNintendo.Website.Controllers
{
    public sealed class GamesController : Controller
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContextFactory _ctxFactory;

        public GamesController(ILogger<HomeController> logger, ApplicationDbContextFactory ctxFactory)
        {
            _logger = logger;
            _ctxFactory = ctxFactory;
        }

        public IActionResult Index()
        {
            var model = new GamesModel(_ctxFactory.Get());

            return View(model);
        }

        public async Task<IActionResult> OnSale()
        {
            var model = new OnSaleModel(_ctxFactory);

            return View("OnSale", model);
        }

        //public async Task<IActionResult> GetGame(Game game) => await GetGameById(game.FsId); // todo: pls fix fsid crap

        public async Task<IActionResult> GetGameById(long id)
        {
            await using var ctx = _ctxFactory.Get();
            var model = new GameModel(ctx, id);
            var priceHistory = await ctx.Trend
                                               .Where(p => p.GameId == model.Game.Id)
                                               .OrderBy(p => p.Created)
                                               .ToListAsync();

            var priceData = priceHistory.Select(p => Math.Round(Convert.ToDouble(p.Price), 2, MidpointRounding.AwayFromZero)).ToList();
            var labels = priceHistory.Select(p => p.Created.ToString("yyyy-MM-dd")).ToList();

            if (priceData.Any())
            {
                priceData.Add(priceData.Last());
                labels.Add(DateTime.UtcNow.ToString("yyyy-MM-dd"));
            }

            // https://github.com/mattosaurus/ChartJSCore
            var chart = new Chart
            {
                Type = Enums.ChartType.Line
            };

            var data = new ChartJSCore.Models.Data() { Labels = labels };

            var dataset = new LineDataset
            {
                Label = $"Price-History {model.Game.Title}",
                Data = priceData,
                Fill = true.ToString(),
                LineTension = 0.1,
                SteppedLine = true.ToString(),
                BackgroundColor = ChartColor.FromRgba(75, 192, 192, 0.4),
                BorderColor = ChartColor.FromRgb(75, 192, 192),
                BorderCapStyle = "butt",
                BorderDash = new List<int>(),
                BorderDashOffset = 0.0,
                BorderJoinStyle = "miter",
                PointBorderColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
                PointBackgroundColor = new List<ChartColor> { ChartColor.FromHexString("#ffffff") },
                PointBorderWidth = new List<int> { 1 },
                PointHoverRadius = new List<int> { 5 },
                PointHoverBackgroundColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
                PointHoverBorderColor = new List<ChartColor> { ChartColor.FromRgb(220, 220, 220) },
                PointHoverBorderWidth = new List<int> { 2 },
                PointRadius = new List<int> { 3 },
                PointHitRadius = new List<int> { 10 },
                SpanGaps = false
            };

            data.Datasets = new List<Dataset> { dataset };
            chart.Data = data;

            ViewData["chart"] = chart;

            return View("Single", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}