Shader "Unlit/Graph"
{
    Properties
    {
		_ColorGraph ("Graph Color", Color) = (1,1,1,1)
		_ColorBack ("Background Color", Color) = (0,0,0,0.5)
		_GraphThickness ("Graph Thickness", Range(0.001, 1.0)) = 0.1
		_GraphSpacing ("Graph Spacing", Range(0.001, 0.1)) = 0.001
		_LerpVal ("Lerp Value", Range(0.0, 1.0)) = 0.0
    }
    SubShader
    {
		/*Cull Off*/ ZWrite Off /*ZTest Always*/ Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			#define BUFFER_SIZE 3

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

			float Line(float2 p, float2 a, float2 b)
			{
				float2 pa = p - a, ba = b - a;
				float h = saturate(dot(pa, ba) / dot(ba, ba));
				float2 d = pa - ba *h;
				return dot(d, d);
			}

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
			float _GraphSpacing;
			int1 _PixelSize;
			float _LerpVal;

			float buffer[BUFFER_SIZE];

            fixed4 frag (v2f f) : SV_Target
            {
				//for (int i = 0; i < BUFFER_SIZE; ++i)
				//{
				//	buffer[i] = (float)i / (float)BUFFER_SIZE;
				//}
				//return float4(f.uv[0], 0, 0, 1);

				buffer[0] = 0.2;
				buffer[1] = 0.5;
				buffer[2] = 0.7;

				int index = f.uv[0] * (BUFFER_SIZE-1);

				float p1 = abs(f.uv[1] - buffer[index]);
				float p2 = abs(f.uv[1] - buffer[index+1]);
				//float pos = f.uv[0] * BUFFER_SIZE / BUFFER_SIZE;
				float val = lerp(p1, p2, _LerpVal);// lerp(p1, p2, 0.5);

				return float4(val, 0, 0, 1);

				return lerp(_ColorGraph, _ColorBack, smoothstep(0.0, _GraphThickness/100, val));

				if (abs(f.uv[1] - buffer[index]) < 0.01)
				{
					return _ColorGraph;
				}
				return _ColorBack;

				//float k = Line(f.uv, float2(0.1, 0.1), float2(0.8, 0.8));
				//for (int i = 0; i < BUFFER_SIZE-1; ++i)
				//{
				//	k *= Line(f.uv, float2(i / BUFFER_SIZE - 1, buffer[i]), float2((i+1) / BUFFER_SIZE - 1, buffer[i]));
				//}
				float k = Line(f.uv,float2(0.0,0.5),float2(0.5,0.5));
				k *= Line(f.uv,float2(0.5,0.5),float2(1.0,0.5));

                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return lerp(_ColorGraph, _ColorBack, smoothstep(0.0, _GraphThickness / 10000, k));
            }
            ENDCG
        }
    }
}
