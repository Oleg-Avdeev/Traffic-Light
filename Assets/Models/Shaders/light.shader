// Upgrade NOTE: upgraded instancing buffer 'MyProperties' to new syntax.

// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Unlit/Signal"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Active ("Active", Float) = 0
        _Blinking ("Blinking", Float) = 0
    }
    SubShader
    {
        Pass
        {
            Tags {"LightMode"="ForwardBase"}
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc" // for UnityObjectToWorldNormal
            #include "UnityLightingCommon.cginc" // for _LightColor0

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 diff : COLOR0;
                fixed blink : TEXCOORD1;
            };


            UNITY_INSTANCING_BUFFER_START(MyProperties)
                UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
                UNITY_DEFINE_INSTANCED_PROP(float, _Active)
                UNITY_DEFINE_INSTANCED_PROP(float, _Blinking)
#define _Color_arr MyProperties
            UNITY_INSTANCING_BUFFER_END(MyProperties)

            v2f vert (appdata v)
            {
                UNITY_SETUP_INSTANCE_ID(v);

                v2f o;
                o.uv = v.texcoord;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.blink = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Blinking);
                o.diff  = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color)
                        * (UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Active) + 0.05);

                return o;
            }
            
            sampler2D _MainTex;
            
            fixed4 frag (v2f i) : SV_Target
            {
                return i.diff * lerp(1, (sin(_Time.w*4) + 3)/4, i.blink);
            }
            
            ENDCG
        }
    }
}