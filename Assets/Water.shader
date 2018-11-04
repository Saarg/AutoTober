Shader "Custom/Water" {
	Properties {
		// color of the water
		_Color("Color", Color) = (1, 1, 1, 1)
		// color of the edge effect
		_EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
		// width of the edge effect
		_DepthFactor("Depth Factor", float) = 1.0
		// wave
		_WaveSpeed("Wave speed", float) = 1.0
		_WaveAmp("Wave amplitude", float) = 1.0
		_NoiseTex("Wave noise", 2D) = "white" { }
    }
    SubShader {
        Pass {

        CGPROGRAM
		// required to use ComputeScreenPos()
		#include "UnityCG.cginc"

        #pragma vertex vert
        #pragma fragment frag

		// Unity built-in - NOT required in Properties
 		sampler2D _CameraDepthTexture;

        #include "UnityCG.cginc"

        float4 _Color;
		float4 _EdgeColor;
		float _DepthFactor;

		float _WaveSpeed;
		float _WaveAmp;
		sampler2D _NoiseTex;

		struct vertexInput
		{
			float4 vertex : POSITION;
            float4 uv : TEXCOORD2;
		};

        struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 screenPos : TEXCOORD1;
        };


        vertexOutput vert (vertexInput input)
        {
            vertexOutput output;
            
			// convert obj-space position to camera clip space
			output.pos = UnityObjectToClipPos(input.vertex);

			float noiseSample = tex2Dlod(_NoiseTex, float4(input.uv.xy, 0, 0));
			output.pos.y += sin(_Time*_WaveSpeed*noiseSample)*_WaveAmp;
			output.pos.x += cos(_Time*_WaveSpeed*noiseSample)*_WaveAmp;

			// compute depth (screenPos is a float4)
			output.screenPos = ComputeScreenPos(output.pos);

            return output;
        }

        float4 frag(vertexOutput input) : COLOR
        {
            float depth = (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(input.screenPos)));
			depth = LinearEyeDepth(depth).r;

			// apply the DepthFactor to be able to tune at what depth values
			// the foam line actually starts
			float foamLine = 1 - saturate(_DepthFactor * (depth - input.screenPos.w));

			// Because the camera depth texture returns a value between 0-1,
			// we can use that value to create a grayscale color
			// to test the value output.
			return _Color + foamLine * _EdgeColor;
        }
        ENDCG
        }
    }
}
