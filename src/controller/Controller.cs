﻿using System;
namespace Florine
{
    // Primary game bottleneck/controller
    public class Controller
    {

        // Loaded from Platform/ Init
        //
        // For pull based:
        // Platform -> Init -> GetCurrentPage() -> display/interact
        //                                            ^       |
        //                                            |       v
        //                                          UserOption(opt)
        //
        // Void Init
        public IPage Init() {
            // Setup - what should it return?
            return null;
        }

        public IPage GetCurrentPage() {
            return _context.CurrentPage;
        }

        public IPage UserOption(IGameOption opt) {
            return _nextPage(opt);
        }

        public Controller(IPlatformFoundry foundry)
        {
            _foundry = foundry;
            _context = _foundry.LoadGameState();
        }

        private IPlatformFoundry _foundry;
        private GameState _context;

        private IPage _getPage(GameState.PageType mainType, GameState.PageSubType subType) {
            //Get Page from Interface Factory
            return _foundry.GetPage(mainType, subType, _context);
        }

        // Dumb Hardcoded Implementation - most options don't matter for the flow
        private IPage _nextPage(IGameOption opt) {
            // Meta Options

            switch(_context.CurrentPage.MainType) {
                case GameState.PageType.Start:
                    // TODO: Actual Switch
                    return _getPage(GameState.PageType.Char_Creation, GameState.PageSubType.Setup);
                    break;
                case GameState.PageType.Char_Creation:
                    return _getPage(GameState.PageType.Day_Intro, GameState.PageSubType.Daily);
                    break;
                case GameState.PageType.Game_Loader:
                    // TODO: Actually Implement
                    return _getPage(GameState.PageType.Select_Meal, GameState.PageSubType.Breakfast);
                    break;
                case GameState.PageType.Day_Intro:
                    return _getPage(GameState.PageType.Select_Meal, GameState.PageSubType.Breakfast);
                    break;
                case GameState.PageType.Select_Meal:
                    return _getPage(GameState.PageType.Summarize_Meal, _context.CurrentPage.SubType);
                    break;
                case GameState.PageType.Summarize_Meal:
                    GameState.PageType nextType = GameState.PageType.Select_Meal;
                    GameState.PageSubType nextSubType = GameState.PageSubType.Daily;
                    switch(_context.CurrentPage.SubType) {
                        case GameState.PageSubType.Breakfast:
                            nextSubType = GameState.PageSubType.Lunch;
                            break;
                        case GameState.PageSubType.Lunch:
                            nextType = GameState.PageType.Select_Activity;
                            break;
                        case GameState.PageSubType.Dinner:
                            nextType = GameState.PageType.Summarize_Day;
                            break;
                    }
                    return _getPage(nextType, nextSubType);
                    break;
                case GameState.PageType.Select_Activity:
                    return _getPage(GameState.PageType.Summarize_Activity, GameState.PageSubType.Daily);
                    break;
                case GameState.PageType.Summarize_Activity:
                    return _getPage(GameState.PageType.Select_Meal, GameState.PageSubType.Dinner);
                    break;
                case GameState.PageType.Summarize_Day:
                    return _getPage(GameState.PageType.Day_Intro, GameState.PageSubType.Daily);
                    break;
                default:
                    return _getPage(GameState.PageType.Start, GameState.PageSubType.Setup);
            }
        }
    }
}
