﻿namespace RayTracer
{
    public static class Transformation
    {
        public static Matrix Translation(float x, float y, float z)
        {
            return new Matrix(
                1, 0, 0, x,
                0, 1, 0, y,
                0, 0, 1, z,
                0, 0, 0, 1);
        }

        public static Matrix Scaling(float x, float y, float z)
        {
            return new Matrix(
                x, 0, 0, 0,
                0, y, 0, 0,
                0, 0, z, 0,
                0, 0, 0, 1);
        }

        public static Matrix RotationX(float radians)
        {
            float sinTheta = System.MathF.Sin(radians);
            float cosTheta = System.MathF.Cos(radians);

            return new Matrix(
               1, 0, 0, 0,
               0, cosTheta, -sinTheta, 0,
               0, sinTheta, cosTheta, 0,
               0, 0, 0, 1);
        }

        public static Matrix RotationY(float radians)
        {
            float sinTheta = System.MathF.Sin(radians);
            float cosTheta = System.MathF.Cos(radians);

            return new Matrix(
               cosTheta, 0, sinTheta, 0,
               0, 1, 0, 0,
               -sinTheta, 0, cosTheta, 0,
               0, 0, 0, 1);
        }

        public static Matrix RotationZ(float radians)
        {
            float sinTheta = System.MathF.Sin(radians);
            float cosTheta = System.MathF.Cos(radians);

            return new Matrix(
               cosTheta, -sinTheta, 0, 0,
               sinTheta, cosTheta, 0, 0,
               0, 0, 1, 0,
               0, 0, 0, 1);
        }

        public static Matrix Shearing(float xY, float xZ, float yX, float yZ, float zX, float zY)
        {
            return new Matrix(
                1, xY, xZ, 0,
                yX, 1, yZ, 0,
                zX, zY, 1, 0,
                0, 0, 0, 1);
        }
    }
}
