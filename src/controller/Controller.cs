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
        public IPlatformFoundry CurrentFoundry { get { return _foundry; } }
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
			_foundry.GetNextGameState(_context, opt, out nextType, out nextSubType);
			return _goToPage(nextType, nextSubType);
        }
    }
}
