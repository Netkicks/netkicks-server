using System;

namespace netkicks_server
{
    class Ball
    {
        public float velX, velY, velZ;
        public float x, y = 20f, z;
        public const float GRAVITY = -0.0021f;
        public float bounce = 0.75f;
        public float friction = 1.001f;
        public const float GROUND_LEVEL = 0.12f;
        public float fixedKickSpeed = 0.028f;
        public float maxKickSpeed = 0.025f;

        public void Update()
        {
            velY += GRAVITY;
            x += velX;
            y += velY;
            z += velZ;

            if (y < GROUND_LEVEL)
            {
                // Has touched the ground.
                y = GROUND_LEVEL;
                velY *= bounce;
                velY *= -1;
            }

            // Apply friction.
            if (GetMagnitude(velX, velZ) < 0.0009f)
            {
                velZ = velX = 0;
            }
            else
            {
                velX /= friction;
                velZ /= friction;
            }
        }

        float GetMagnitude(float x, float z)
        {
            return (float) Math.Sqrt((x * x) + (z * z));
        }

    }
}
