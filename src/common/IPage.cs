using System;
namespace Florine
{
    public interface IPage
    {
        GameState.PageType    MainType { get; }
        GameState.PageSubType SubType { get; }
    }
}
