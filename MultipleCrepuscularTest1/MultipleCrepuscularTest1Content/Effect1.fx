float4x4 Projection;

Texture ColorMap;
sampler ColorMapSampler = sampler_state 
{
	texture = <ColorMap>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = mirror;
	AddressV = mirror;
};

struct VertexToPixel
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float4 Color : COLOR0;
};

struct PixelToFrame
{
	float4 Color : COLOR0;
};

VertexToPixel MyVertexShader(float4 inPos: POSITION0, float2 texCoord: TEXCOORD0, float4 color: COLOR0)
{
	VertexToPixel Output = (VertexToPixel)0;

	//Output.Position = inPos;
	Output.Position = mul(inPos, Projection);
	Output.TexCoord = texCoord;
	Output.Color = color;
	
	return Output;
}

PixelToFrame PointLightShader(VertexToPixel PSIn) : COLOR0
{
	PixelToFrame Output = (PixelToFrame)0;
	float4 colorMap = tex2D(ColorMapSampler, PSIn.TexCoord);

	Output.Color = colorMap;
	return Output;
}

technique DeferredPointLight
{
    pass Pass1
    {
		VertexShader = compile vs_3_0 MyVertexShader();
        PixelShader = compile ps_3_0 PointLightShader();
    }
}

