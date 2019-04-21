using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;
using System;

using Chaotx.Mgx.Control.Menu;
using Chaotx.Mgx.Control;
using Chaotx.Mgx.Layout;

namespace Chaotx.Minesweeper {
    public class HighscoreView : GameView {
        public int EntriesPerPage {get; set;} = 10;

        private Minesweeper game;
        private List<VPane> pages;

        private SpriteFont font;
        private Texture2D blank;
        private Texture2D arrLeft;
        private Texture2D arrRight;
        private ImageItem background;

        private int selectedPage;
        private int maxPages;

        private StackPane stack;

        public HighscoreView(Minesweeper game) : base(game.Content, game.GraphicsDevice) {
            font = Content.Load<SpriteFont>("fonts/menu_font");
            blank = Content.Load<Texture2D>("textures/blank");
            arrLeft = Content.Load<Texture2D>("textures/arrow_left");
            arrRight = Content.Load<Texture2D>("textures/arrow_right");
            background = new ImageItem(blank);
            background.HGrow = background.VGrow = 1;
            background.Color = Color.Black;
            background.Alpha = 0.5f;
            this.game = game;
            Init();
        }

        public void Init() {
            int i = 1;
            int p = Int32.MaxValue;

            pages = new List<VPane>();
            VPane page = null;

            game.Scores.ForEach(score => {
                if(p > EntriesPerPage) {
                    page = new VPane();
                    page.HGrow = page.VGrow = 1;
                    pages.Add(page);
                    p = 1;
                }

                TextItem index = new TextItem(font, i.ToString("{0:00}: "));
                TextItem diff = new TextItem(font, score.Settings.Difficulty.ToString());
                TextItem name = new TextItem(font, score.Name);
                TextItem time = new TextItem(font, score.Time.ToString(@"\mm\:ss\.ff"));
                TextItem hits = new TextItem(font, score.MinesHit.ToString("{0:00}"));
                TextItem total = new TextItem(font, score.TotalMines.ToString("/{0:00}"));
                index.HAlign = diff.HAlign = name.HAlign = time.HAlign = hits.HAlign = total.HAlign = HAlignment.Center;

                HPane hIndex = new HPane(index);
                HPane hDiff = new HPane(diff);
                HPane hName = new HPane(name);
                HPane hTime = new HPane(time);
                HPane hHits = new HPane(hits, total);
                hDiff.HGrow = hName.HGrow = hTime.HGrow = hHits.HGrow = 2;
                hIndex.HGrow = 1;

                HPane hPane = new HPane(hDiff, hName, hTime, hHits);
                hPane.HGrow = 1;
                page.Add(hPane);
                ++p;
                ++i;
            });

            MenuItem left = new MenuItem(arrLeft, 32, 32);
            MenuItem right = new MenuItem(arrRight, 32, 32);
            MenuItem back = new MenuItem("Back", font);
            left.VAlign = right.VAlign = VAlignment.Center;
            back.HAlign = HAlignment.Center;
            back.VAlign = VAlignment.Top;

            left.Disabled += (s, a) => {
                if(left.IsDisabled) {
                    left.Color = Color.Gray;
                    left.Alpha = 0.8f;
                } else {
                    left.Color = Color.White;
                    left.Alpha = 1;
                }
            };

            right.Disabled += (s, a) => {
                if(right.IsDisabled) {
                    right.Color = Color.Gray;
                    right.Alpha = 0.8f;
                } else {
                    right.Color = Color.White;
                    right.Alpha = 1;
                }
            };

            left.IsDisabled = true;
            if(pages.Count == 0) right.IsDisabled = true;

            stack = new StackPane(background);
            stack.HGrow = stack.VGrow = 1;
            if(pages.Count > 0) stack.Add(pages[0]);

            HPane hpLeft = new HPane(left);
            HPane hpCent = new HPane(stack);
            HPane hpRight = new HPane(right);
            hpLeft.HAlign = hpRight.HAlign = hpCent.HAlign = HAlignment.Center;
            hpLeft.VAlign = hpRight.VAlign = VAlignment.Center;
            hpCent.HGrow = 0.8f;
            hpCent.VGrow = 1;

            HPane hMain = new HPane(hpLeft, hpCent, hpRight);
            VPane vMain = new VPane(hMain, back);
            hMain.HGrow = hMain.VGrow = 1;
            vMain.HGrow = vMain.VGrow = 1;
            MainContainer.Add(vMain);

            left.FocusGain += (s, a) => left.Image.Color = Color.Yellow;
            left.FocusLoss += (s, a) => left.Image.Color = Color.White;
            right.FocusGain += (s, a) => right.Image.Color = Color.Yellow;
            right.FocusLoss += (s, a) => right.Image.Color = Color.White;
            back.FocusGain += (s, a) => back.Text.Color = Color.Yellow;
            back.FocusLoss += (s, a) => back.Text.Color = Color.White;

            left.Action += (s, a) => {
                selectedPage = Math.Max(0, selectedPage-1);
                if(selectedPage < pages.Count)
                    SwapPage(pages[selectedPage]);

                right.IsDisabled = selectedPage >= pages.Count-1;
                left.IsDisabled = selectedPage <= 0;
            };

            right.Action += (s, a) => {
                selectedPage = Math.Min(pages.Count-1, selectedPage+1);
                if(selectedPage >= 0)
                    SwapPage(pages[selectedPage]);

                right.IsDisabled = selectedPage >= pages.Count-1;
                left.IsDisabled = selectedPage <= 0;
            };

            back.Action += (s, a) => Close();
        }

        private void SwapPage(VPane newPage) {
            int c = stack.Children.Count;
            if(c > 1) stack.Remove(stack.Children[c-1]);
            stack.Add(newPage);
        }
    }
}