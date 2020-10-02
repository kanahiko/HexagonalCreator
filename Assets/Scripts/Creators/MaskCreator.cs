using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaskCreator
{
    static Texture2D mask;
    static Material maskMaterial;
    static Texture2D borderMask;
    static Material borderMaskMaterial;
    static int currentCount = 0;
    static int currentBorderCount = 0;

    static Color neutralColor = Color.white;
    static Color blueColor = new Color32(91, 110,225,255);
    static Color redColor = new Color32(217, 87, 99, 255);

    public static void InitializeCreator(Material material, Material borderMaterial)
    {
        maskMaterial = material;
        borderMaskMaterial = borderMaterial;
    }

    public static void CreateMask(int countries, int count)
    {
        if (currentCount != count)
        {
            mask = new Texture2D(count, 1,TextureFormat.ARGB32, false);
            currentCount = count;
        }

        for(int i = 0; i < count; i++)
        {
            if (((countries>>i)&1) == 1)
            {
                mask.SetPixel(i, 0, Color.white);
            }
            else
            {
                mask.SetPixel(i, 0, Color.black);
            }
        }

        mask.Apply();
        maskMaterial.mainTexture = mask;
    }

    public static void CreateZeroMask(int count)
    {
        if (maskMaterial == null)
        {
            return;
        }
        if (currentCount != count)
        {
            mask = new Texture2D(count, 1, TextureFormat.ARGB32, false);
            currentCount = count;
        }

        for (int i = 0; i < count; i++)
        {
            mask.SetPixel(i, 0, Color.black);
        }

        mask.Apply();
        maskMaterial.mainTexture = mask;
    }

    public static void CreateBorderMask(int displayColor, int color, int war, int count)
    {
        if (currentBorderCount != count)
        {
            borderMask = new Texture2D(count, 2, TextureFormat.ARGB32, false);
            currentBorderCount = count;
        }

        for (int i = 0; i < count; i++)
        {
            if (((displayColor >> i) & 1) == 0)
            {
                borderMask.SetPixel(i, 0, neutralColor);
                borderMask.SetPixel(i, 1, neutralColor);
            }
            else
            {
                bool isBlue = ((color >> i) & 1) == 1;
                if (((war >> i) & 1) == 1)
                {
                    if (isBlue)
                    {
                        borderMask.SetPixel(i, 0, blueColor);
                        borderMask.SetPixel(i, 1, redColor);
                    }
                    else
                    {
                        borderMask.SetPixel(i, 0, redColor);
                        borderMask.SetPixel(i, 1, blueColor);
                    }
                }
                else
                {
                    if (isBlue)
                    {
                        borderMask.SetPixel(i, 0, blueColor);
                        borderMask.SetPixel(i, 1, blueColor);
                    }
                    else
                    {
                        borderMask.SetPixel(i, 0, redColor);
                        borderMask.SetPixel(i, 1, redColor);
                    }
                }
            }
        }

        borderMask.Apply();
        borderMaskMaterial.mainTexture = borderMask;
    }
}
