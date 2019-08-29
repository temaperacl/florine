using System;
using Florine;

namespace FlorineWeb
{
    public class WebFoundry : IPlatformFoundry
    {
        /* IPlatformFoundry */
        public GameState LoadGameState()
        {
            return new GameState();
        }

        public IPage GetPage(IPage GenericPage)
        {
            return GenericPage;
        }

        /* WebFoundry */
        public System.Web.UI.Control RenderPage(IPage Source) { return RenderPage(Source, false); }
        public System.Web.UI.Control RenderPage(IPage Source, bool debugInfo)
        {
            System.Web.UI.WebControls.Panel Body = new System.Web.UI.WebControls.Panel();        
            if (debugInfo) {
                Body.Controls.Add(new System.Web.UI.WebControls.Label()
                {
                    Text = Source.MainType.ToString() + "(" + Source.SubType.ToString() + ")"
                });
            }
            if (Source.Background != null)
            {
                Body.Controls.Add(ImageFromObject(Source.Background));
            }
            if (Source.Title != null) {
                Body.Controls.Add(new System.Web.UI.HtmlControls.HtmlGenericControl("h3")
                {
                    InnerHtml = Source.Title
                });
            }
            if (Source.Message != null)
            {
                System.Web.UI.WebControls.Panel MessagePanel = new System.Web.UI.WebControls.Panel();
                MessagePanel.Controls.Add(new System.Web.UI.WebControls.Label() { Text = Source.Message });
                Body.Controls.Add(MessagePanel);
            }

            // ToDo: NutrientState/NutrientDelta;
            // ToDo: PrimaryOptions

            return Body;
        }

        public System.Web.UI.WebControls.Image ImageFromObject(IImage Source) {
            return new System.Web.UI.WebControls.Image();
        }
    }
}
