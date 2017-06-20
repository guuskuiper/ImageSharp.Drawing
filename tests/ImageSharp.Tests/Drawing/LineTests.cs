﻿// <copyright file="ColorConversionTests.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests.Drawing
{
    using ImageSharp.Drawing;
    using ImageSharp.Drawing.Pens;

    using System.IO;
    using System.Numerics;

    using ImageSharp.PixelFormats;

    using Xunit;

    public class LineTests : FileTestBase
    {
        [Fact]
        public void ImageShouldBeOverlayedByPath()
        {
            string path = this.CreateOutputDirectory("Drawing", "Lines");
            using (Image<Rgba32> image = new Image<Rgba32>(500, 500))
            {
                image
                    .BackgroundColor(Rgba32.Blue)
                    .DrawLines(Rgba32.HotPink, 5,
                    new[] {
                            new Vector2(10, 10),
                            new Vector2(200, 150),
                            new Vector2(50, 300)
                    })
                    .Save($"{path}/Simple.png");

                using (PixelAccessor<Rgba32> sourcePixels = image.Lock())
                {
                    Assert.Equal(Rgba32.HotPink, sourcePixels[11, 11]);

                    Assert.Equal(Rgba32.HotPink, sourcePixels[199, 149]);

                    Assert.Equal(Rgba32.Blue, sourcePixels[50, 50]);
                }
            }
        }

        [Fact]
        public void ImageShouldBeOverlayedByPath_NoAntialias()
        {
            string path = this.CreateOutputDirectory("Drawing", "Lines");
            using (Image<Rgba32> image = new Image<Rgba32>(500, 500))
            {
                image
                    .BackgroundColor(Rgba32.Blue)
                    .DrawLines(Rgba32.HotPink, 5,
                    new[] {
                            new Vector2(10, 10),
                            new Vector2(200, 150),
                            new Vector2(50, 300)
                    },
                    new GraphicsOptions(false))
                    .Save($"{path}/Simple_noantialias.png");

                using (PixelAccessor<Rgba32> sourcePixels = image.Lock())
                {
                    Assert.Equal(Rgba32.HotPink, sourcePixels[11, 11]);

                    Assert.Equal(Rgba32.HotPink, sourcePixels[199, 149]);

                    Assert.Equal(Rgba32.Blue, sourcePixels[50, 50]);
                }
            }
        }

        [Fact]
        public void ImageShouldBeOverlayedByPathDashed()
        {
            string path = this.CreateOutputDirectory("Drawing", "Lines");
            using (Image<Rgba32> image = new Image<Rgba32>(500, 500))
            {
                image
                    .BackgroundColor(Rgba32.Blue)
                    .DrawLines(Pens.Dash(Rgba32.HotPink, 5),
                    new[] {
                            new Vector2(10, 10),
                            new Vector2(200, 150),
                            new Vector2(50, 300)
                    })
                    .Save($"{path}/Dashed.png");
            }
        }

        [Fact]
        public void ImageShouldBeOverlayedByPathDotted()
        {
            string path = this.CreateOutputDirectory("Drawing", "Lines");
            using (Image<Rgba32> image = new Image<Rgba32>(500, 500))
            {
                image
                    .BackgroundColor(Rgba32.Blue)
                    .DrawLines(Pens.Dot(Rgba32.HotPink, 5),
                    new[] {
                            new Vector2(10, 10),
                            new Vector2(200, 150),
                            new Vector2(50, 300)
                    })
                    .Save($"{path}/Dot.png");
            }
        }

        [Fact]
        public void ImageShouldBeOverlayedByPathDashDot()
        {
            string path = this.CreateOutputDirectory("Drawing", "Lines");
            using (Image<Rgba32> image = new Image<Rgba32>(500, 500))
            {
                image
                    .BackgroundColor(Rgba32.Blue)
                    .DrawLines(Pens.DashDot(Rgba32.HotPink, 5),
                    new[] {
                            new Vector2(10, 10),
                            new Vector2(200, 150),
                            new Vector2(50, 300)
                    })
                    .Save($"{path}/DashDot.png");
            }
        }

        [Fact]
        public void ImageShouldBeOverlayedByPathDashDotDot()
        {
            string path = this.CreateOutputDirectory("Drawing", "Lines");
            Image<Rgba32> image = new Image<Rgba32>(500, 500);

            image
                .BackgroundColor(Rgba32.Blue)
                .DrawLines(Pens.DashDotDot(Rgba32.HotPink, 5), new[] {
                            new Vector2(10, 10),
                            new Vector2(200, 150),
                            new Vector2(50, 300)
                })
                .Save($"{path}/DashDotDot.png");
        }

        [Fact]
        public void ImageShouldBeOverlayedPathWithOpacity()
        {
            string path = this.CreateOutputDirectory("Drawing", "Lines");

            Rgba32 color = new Rgba32(Rgba32.HotPink.R, Rgba32.HotPink.G, Rgba32.HotPink.B, 150);

            Image<Rgba32> image = new Image<Rgba32>(500, 500);

            image
                .BackgroundColor(Rgba32.Blue)
                .DrawLines(color, 10, new[] {
                            new Vector2(10, 10),
                            new Vector2(200, 150),
                            new Vector2(50, 300)
                })
                .Save($"{path}/Opacity.png");

            //shift background color towards forground color by the opacity amount
            Rgba32 mergedColor = new Rgba32(Vector4.Lerp(Rgba32.Blue.ToVector4(), Rgba32.HotPink.ToVector4(), 150f / 255f));

            using (PixelAccessor<Rgba32> sourcePixels = image.Lock())
            {
                Assert.Equal(mergedColor, sourcePixels[11, 11]);

                Assert.Equal(mergedColor, sourcePixels[199, 149]);

                Assert.Equal(Rgba32.Blue, sourcePixels[50, 50]);
            }
        }

        [Fact]
        public void ImageShouldBeOverlayedByPathOutline()
        {
            string path = this.CreateOutputDirectory("Drawing", "Lines");

            Image<Rgba32> image = new Image<Rgba32>(500, 500);

            image
                .BackgroundColor(Rgba32.Blue)
                .DrawLines(Rgba32.HotPink, 10, new[] {
                            new Vector2(10, 10),
                            new Vector2(200, 10),
                            new Vector2(200, 150),
                            new Vector2(10, 150)
                    })
                .Save($"{path}/Rectangle.png");

            using (PixelAccessor<Rgba32> sourcePixels = image.Lock())
            {
                Assert.Equal(Rgba32.HotPink, sourcePixels[11, 11]);

                Assert.Equal(Rgba32.HotPink, sourcePixels[198, 10]);

                Assert.Equal(Rgba32.Blue, sourcePixels[10, 50]);

                Assert.Equal(Rgba32.Blue, sourcePixels[50, 50]);
            }
        }

    }
}
