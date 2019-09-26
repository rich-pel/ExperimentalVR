Shader "Unlit/Graph"
{
    Properties
    {
		_ColorGraph ("Graph Color", Color) = (1,1,1,1)
		_ColorBack ("Background Color", Color) = (0,0,0,0.5)
		_GraphThickness ("Graph Thickness", Range(0.001, 1.0)) = 0.1
		_Range ("Range", Range(1, 1024)) = 1024 // must macth MAX_BUFFER_SIZE !!!
    }
    SubShader
    {
		/*Cull Off*/ ZWrite Off /*ZTest Always*/ Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			#define MAX_BUFFER_SIZE 1024

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float4 _ColorGraph;
			float4 _ColorBack;
			float _GraphThickness;

			float _ValueBuffer[MAX_BUFFER_SIZE];
			int _Range;

            fixed4 frag (v2f f) : SV_Target
            {
				//for (int i = 0; i < BUFFER_SIZE; ++i)
				//{
				//	buffer[i] = (float)i / (float)(BUFFER_SIZE-1);
				//}

				float space = 1.0 / (_Range -1);	// space between two values
				int index = f.uv[0] * (_Range -1);	// current index, depending on x positon

				float pos = f.uv[0] - (index * space);	// position relative to our index
				float inter = lerp(_ValueBuffer[index], _ValueBuffer[index + 1], (pos / space));	// lerp between this and next value, depending on our relative position
				float val = abs(f.uv[1] - inter);		// determine whether we are close to our interpolated value (y position)

				// draw in respect to line thickness
				return lerp(_ColorGraph, _ColorBack, smoothstep(0.0, _GraphThickness/10, val));
            }
            ENDCG
        }
    }
}
