namespace TextureGen.Generation;

public interface IGenerator<in TParameters>
{
    Texture Generate(TParameters parameters);
}