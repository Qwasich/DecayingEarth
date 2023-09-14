using DecayingEarth;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utility
{
    public class UsefulBits : MonoBehaviour
    {

        /// <summary>
        /// ������ ���� �� ������������ ����������� �� ������������ ��������
        /// </summary>
        /// <param name="tileCoord">���������� ����� �� �����</param>
        /// <param name="targetTilemap">������� �������</param>
        /// <param name="block">����, ������� ����� �����������</param>
        /// <param name="shakeTime">����� ����� ������ �������</param>
        /// <param name="shakeIntensity">������������� ������</param>
        /// <param name="timesPerSec">�������� � �������</param>
        /// <param name="randomSeed">���� ��� ����������� ������</param>
        /// <returns></returns>
        public static IEnumerator TileShaker(Vector3Int tileCoord, Tilemap targetTilemap, TileBlockBase block, float shakeTime, float shakeIntensity, float timesPerSec, int randomSeed = 0)
        {
            Random.InitState(randomSeed);
            int cap = 0;

            float yieldTimer = 1 / timesPerSec;
            float timer = shakeTime;

            float frameStart = Time.frameCount;
            float frameSinceYield = 1;

            while (timer > 0 && cap < 10000)
            {

                
                Matrix4x4 tileTransform = Matrix4x4.Translate(Random.insideUnitCircle * shakeIntensity);

                TileChangeData tileData = new TileChangeData
                {
                    position = tileCoord,
                    tile = block,
                    color = Color.white,
                    transform = tileTransform
                };

                targetTilemap.SetTile(tileData, false);

                timer -= Time.deltaTime * frameSinceYield;
                ;
                cap++;

                yield return new WaitForSeconds(yieldTimer);

                frameSinceYield = Time.frameCount - frameStart;
                frameStart = Time.frameCount;
            }

            Matrix4x4 tl = Matrix4x4.Translate(Vector3.zero);

            TileChangeData td= new TileChangeData
            {
                position = tileCoord,
                tile = block,
                color = Color.white,
                transform = tl
            };

            targetTilemap.SetTile(td, false);
        }

        public static void FixTilePosition(Vector3Int tileCoord, Tilemap targetTilemap, TileBlockBase block)
        {
            Matrix4x4 tl = Matrix4x4.Translate(Vector3.zero);

            TileChangeData td = new TileChangeData
            {
                position = tileCoord,
                tile = block,
                color = Color.white,
                transform = tl
            };

            targetTilemap.SetTile(td, false);
        }


        public static Color GetAverageSpriteColor(Sprite spr)
        {
            Color[] colors = spr.texture.GetPixels();
            var total = colors.Length;
            float r = 0; float g = 0; float b = 0;

            for (var i = 0; i < total; i++)
            {
                r += colors[i].r;
                g += colors[i].g;
                b += colors[i].b;
            }
            return new Color(r / total, g / total, b / total);

        }
    }
}
