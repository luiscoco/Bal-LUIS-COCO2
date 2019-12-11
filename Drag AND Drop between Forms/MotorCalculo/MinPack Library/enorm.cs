using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MINPACK
{
    /// <summary>
    /// For further information view the MinPack documentation.
    /// </summary>
    public class EnormClass
    {
        /// <summary>
        /// For further information view the MinPack documentation.
        /// </summary>
        /// <param name="n">The n parameter.</param>
        /// <param name="x">The x parameter.</param>
        /// <returns>For further information view the MinPack documentation.</returns>
        public double Enorm(int n, double[] x)
        {
            double sum = x[0] * x[0];

            for (int i = 1; i < n; i++)
            {
                sum += x[i] * x[i];
            }

            return Math.Sqrt(sum);
        }

        /// <summary>
        /// For further information view the MinPack documentation.
        /// </summary>
        /// <param name="m">The m parameter.</param>
        /// <param name="r">The r parameter.</param>
        /// <param name="c">The c parameter.</param>
        /// <param name="x">The x parameter.</param>
        /// <returns>For further information view the MinPack documentation.</returns>
        public double Rownorm(int m, int r, int c, double[,] x)
        {
            double sum = x[r, c] * x[r, c];

            for (int i = r + 1; i < m; i++)
            {
                sum += x[i, c] * x[i, c];
            }

            return Math.Sqrt(sum);
        }

        /// <summary>
        /// For further information view the MinPack documentation.
        /// </summary>
        /// <param name="m">The m parameter.</param>
        /// <param name="r">The r parameter.</param>
        /// <param name="c">The c parameter.</param>
        /// <param name="x">The x parameter.</param>
        /// <returns>For further information view the MinPack documentation.</returns>
        public double Colnorm(int m, int r, int c, double[,] x)
        {
            double sum = x[r, c] * x[r, c];

            for (int i = c + 1; i < m; i++)
            {
                sum += x[r, i] * x[r, i];
            }

            return Math.Sqrt(sum);
        }
    }
}
