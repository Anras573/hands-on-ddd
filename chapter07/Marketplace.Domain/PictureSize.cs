namespace Marketplace.Domain;

public record PictureSize
{
    public int Height { get; }
    public int Width { get; }
    
    public PictureSize(int width, int height)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width), "Picture width must be a positive number");

        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height),"Picture height must be a positive number");

        Width = width;
        Height = height;
    }
}