using System;
namespace Florine
{
    // Primary game bottleneck/controller
    public class Controller
    {

        private IPlatformFoundry _foundry;
        private GameState _context;

        public Controller(IPlatformFoundry foundry)
        {
            _foundry = foundry;            
            _context = _foundry.LoadGameState();
        }
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
            _foundry.LoadFood();
            return GetCurrentPage();
        }

        public GameState CurrentState { get { return _context; } }

        public IPage GetCurrentPage() {
            return _foundry.GetPage(_context.CurrentPage);
        }

        public IPage UserOption(IGameOption opt) {
            return _nextPage(opt);
        }

        private IPage _goToPage(GameState.PageType mainType, GameState.PageSubType subType) {            
            //Get Page from Interface Factory
            _context.SetPage(mainType, subType);
			Activity autoActivity = _foundry.AutomaticActivity(_context);
			if(null != autoActivity) {
			    _context.ApplyOption(autoActivity);
			}
            return GetCurrentPage();
        }

        // Dumb Hardcoded Implementation - most options don't matter for the flow
        private IPage _nextPage(IGameOption opt) {
            // Meta Options
            _context.ReadyNextPage();
			if(null != opt) {
            	_context.ApplyOption(opt);
			}
			GameState.PageType nextType;
            GameState.PageSubType nextSubType;
			
            switch(_context.CurrentPage.MainType) {
                case GameState.PageType.Start:
                    // TODO: Actual Switch
                    return _goToPage(GameState.PageType.Char_Creation, GameState.PageSubType.Setup);
                    break;
                case GameState.PageType.Char_Creation:
                    return _goToPage(GameState.PageType.Day_Intro, GameState.PageSubType.Daily);
                    break;
                case GameState.PageType.Game_Loader:
                    // TODO: Actually Implement
                    return _goToPage(GameState.PageType.Select_Meal, GameState.PageSubType.Breakfast);
                    break;
                case GameState.PageType.Day_Intro:
                    return _goToPage(GameState.PageType.Select_Meal, GameState.PageSubType.Breakfast);
                    break;
                case GameState.PageType.Select_Meal:
                    return _goToPage(GameState.PageType.Summarize_Meal, _context.CurrentPage.SubType);
                    break;
                case GameState.PageType.Summarize_Meal:
                    nextType = GameState.PageType.Summarize_Activity;
                    nextSubType = _context.CurrentPage.SubType;
                    return _goToPage(nextType, nextSubType);
                    break;
                case GameState.PageType.Select_Activity:
                    return _goToPage(GameState.PageType.Summarize_Activity, GameState.PageSubType.Daily);
                    break;
                case GameState.PageType.Summarize_Activity:
					nextType = GameState.PageType.Start;
					nextSubType = GameState.PageSubType.Setup;
					switch(_context.CurrentPage.SubType) {
                        case GameState.PageSubType.Breakfast:
							nextType = GameState.PageType.Select_Meal;
                            nextSubType = GameState.PageSubType.Lunch;
                            break;
                        case GameState.PageSubType.Dinner:
                            nextType = GameState.PageType.Select_Activity;
							nextSubType = GameState.PageSubType.Daily;
                            break;
                        case GameState.PageSubType.Lunch:
							nextType = GameState.PageType.Select_Meal;
                            nextSubType = GameState.PageSubType.Dinner;
                            break;							
						case GameState.PageSubType.Daily:
							nextType = GameState.PageType.Summarize_Day;
							nextSubType = GameState.PageSubType.Daily;
							break;
                    }
					return _goToPage(nextType, nextSubType);                    
                    break;
				case GameState.PageType.Summarize_Day:
					return _goToPage(GameState.PageType.Day_Intro, GameState.PageSubType.Daily);
					break;
                default:
                    return _goToPage(GameState.PageType.Start, GameState.PageSubType.Setup);
            }
        }
    }
}
