float4x4 Projection;
float4x4 LightProjection;

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

Texture OcclusionMap;
sampler OcclusionMapSampler = sampler_state 
{
	texture = <OcclusionMap>;
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

	Output.Position = mul(inPos, Projection);
	Output.TexCoord = texCoord;
	Output.Color = color;
	
	return Output;
}


float2 LightPosition = float2(0.51, 0.5);
float decay= 0.9999;
float exposure=0.13;
float density=0.826;
float weight=0.358767;
const int NUM_SAMPLES = 120;


PixelToFrame PointLightShader(VertexToPixel PSIn) : COLOR0
{
	PixelToFrame Output = (PixelToFrame)0;
	float4 colorMap = tex2D(ColorMapSampler, PSIn.TexCoord);
	float occMap = tex2D(OcclusionMapSampler, PSIn.TexCoord).a;

	

	float4 col = float4(0,0,0,0);
	
	if (occMap > 0.0f)
	{
		col = float4(0,0,0,1);
	}
	else
	{
		col = colorMap;
	}

	Output.Color = col;
	return Output;

	//float2 tc = PSIn.TexCoord.xy;
	//float2 deltaTexCoord = (tc - LightPosition.xy);

	//for (int i = 0; i < NUM_SAMPLES; i++)
	//{
	//	tc -= deltaTexCoord;
	//}

	//Output.Color = tex2D(ColorMapSampler, tc.xy);
}

technique DeferredPointLight
{
    pass Pass1
    {
		VertexShader = compile vs_3_0 MyVertexShader();
        PixelShader = compile ps_3_0 PointLightShader();
    }
}

