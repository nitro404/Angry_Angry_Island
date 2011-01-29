sampler2D buffer;

float blurDistance = 0.005f;

float4 BlurPixelShader(float2 tex : TEXCOORD0) : COLOR0 {
	float4 colour;
	
	int blurAmount = 3;
	
	colour = tex2D(buffer, tex.xy);
	for(int i=0;i<blurAmount;i++) {
		float d = blurDistance * (float) i;
		colour += tex2D(buffer, float2(tex.x - d, tex.y));
		colour += tex2D(buffer, float2(tex.x + d, tex.y));
		colour += tex2D(buffer, float2(tex.x, tex.y - d));
		colour += tex2D(buffer, float2(tex.x, tex.y + d));
	}
	colour /= (float) blurAmount * 4.0f;
	
	return colour;
}

technique PostProcess {
	pass Custom {
		PixelShader = compile ps_2_0 BlurPixelShader();
	}
}
