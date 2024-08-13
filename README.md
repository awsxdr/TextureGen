# TextureGen
A .NET library for generating textures

## Example usage

```csharp
	var bricks = Generate.Bricks(
		ImageSize.Size256, 
		new BrickGenerator.Parameters
		{
			Rows = 4,
			Columns = 4,
			MinimumHeight = 0.5f,
			HeightVariance = 0.5f,
			BrickHeight = 0.95f,
			BrickWidth = 0.95f,
			Offset = 0.5f,
		})
		.ScaleToSizeBilinear(ImageSize.Size32);
		
	var noise = 
        Generate.SimpleNoise(bricks.ImageSize)
        .CompressRange(.85f, 1f);

	var color = Generate.Color(
        bricks.ImageSize, 
        new ColorGenerator.Parameters { 
            Color = TextureGen.Color.FromArgb(255, 255, 255, 255) 
        });

	var texture = bricks.CompressRange(.5f, 1f) * color * noise;
```