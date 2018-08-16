using System;
using System.Diagnostics; // to use Stopwatch

namespace MyECS {
    /**
     * Conclusion:
     *      + Naive approach is usually worst
     *      + Smaller context, better algorithm
     *      + We don't need to find BEST solution all the time
     *      + Combine algorithm with data structure
     */
    public class TestGoodEnoughAlgorithms {
        private static Stopwatch stopwatch;
        private static Random randomizer;
        private const int MIN_RANDOM_VALUE     = 0;
        private const int MAX_RANDOM_VALUE     = 1000;
        private const float RANDOM_FACTOR      = 0.1f;
        private const float TOO_CLOSE_DISTANCE = 0.5f;
        private const float TOO_FAR_DISTANCE   = 20f;
        private const int ELEMENTS_COUNT       = 10_000_000;
        //----------------------------------------------------------------------
        public static void Execute () {
            Console.WriteLine("Begin.");
            stopwatch  = new Stopwatch();
            randomizer = new Random();

            // Prepare data
            Transform[] objects = new Transform[ELEMENTS_COUNT];
            Vector3[] positions = new Vector3[ELEMENTS_COUNT];
            for (int i = 0; i < ELEMENTS_COUNT; ++i) {
                positions[i] = GetRandom();
                objects[i]   = new Transform(i, positions[i]);
            }
            Vector3 center = GetRandom();
            
            // Profile
            Console.WriteLine("OOP_MinDistance:                         " + ProfileFindClosestObject_MinDistance(objects, center) + "ms");
            Console.WriteLine("OOP_MinSquareDistance:                   " + ProfileFindClosestObject_MinSquareDistance(objects, center) + "ms");
            Console.WriteLine("OOP_MinSquareDistanceWithConstraints:    " + ProfileFindClosestObject_MinSquareDistanceWithConstraints(objects, center) + "ms");
            Console.WriteLine("OOP_MinSquareDistanceWithConstraints2:   " + ProfileFindClosestObject_MinSquareDistanceWithConstraints2(objects, center) + "ms");
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("DOD_MinDistance:                         " + ProfileFindClosestObject_MinDistanceDOD(positions, center) + "ms");
            Console.WriteLine("DOD_MinDistanceWithFullOptimizations:    " + ProfileFindClosestObject_MinDistanceDODOptimized(positions, center) + "ms");

            Console.WriteLine("End.");
        }
        //----------------------------------------------------------------------
        /**
         * Naive OOP approach: just compare distance to find min value
         */
        private static long ProfileFindClosestObject_MinDistance (Transform[] objects, Vector3 center) {
            stopwatch.Restart();
            int closestId     = 0;
            float minDistance = GetDistanceBetween(center, objects[closestId].position);
            float distance;
            for (int i = 1; i < ELEMENTS_COUNT; ++i) {
                distance = GetDistanceBetween(center, objects[i].position);
                if (minDistance > distance) {
                    minDistance = distance;
                    closestId   = i;
                }
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        //----------------------------------------------------------------------
        /**
         * Improve #1: Use squared distance to compare intead of square root
         * Difference: line 57 vs 77
         */
        private static long ProfileFindClosestObject_MinSquareDistance (Transform[] objects, Vector3 center) {
            stopwatch.Restart();
            int closestId           = 0;
            float minSquareDistance = GetSquareDistanceBetween(center, objects[closestId].position);
            float squareDistance;
            for (int i = 1; i < ELEMENTS_COUNT; ++i) {
                squareDistance = GetSquareDistanceBetween(center, objects[i].position);
                if (minSquareDistance > squareDistance) {
                    minSquareDistance = squareDistance;
                    closestId         = i;
                }
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        //----------------------------------------------------------------------
        /**
         * Improve #2: Add constraints to skip the loop or break the loop sooner
         */
        private static long ProfileFindClosestObject_MinSquareDistanceWithConstraints (Transform[] objects, Vector3 center) {
            stopwatch.Restart();
            int closestId           = 0;
            float minSquareDistance = GetSquareDistanceBetween(center, objects[closestId].position);
            float squareDistance;
            const float MIN_SQUARE_DISTANCE = TOO_CLOSE_DISTANCE * TOO_CLOSE_DISTANCE;
            const float MAX_SQUARE_DISTANCE = TOO_FAR_DISTANCE * TOO_FAR_DISTANCE;
            for (int i = 1; i < ELEMENTS_COUNT; ++i) {
                squareDistance = GetSquareDistanceBetween(center, objects[i].position);
                if (squareDistance < MIN_SQUARE_DISTANCE) {
                    closestId = i;
                    break;      // this happens ONCE -> Branch misprediction
                }
                if (squareDistance > MAX_SQUARE_DISTANCE) {
                    continue;   // this happens many times
                }
                if (minSquareDistance > squareDistance) {
                    minSquareDistance = squareDistance;
                    closestId = i;
                }
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        //----------------------------------------------------------------------
        /**
         * Improve #3: Reposition branch to make it skip sooner
         * Differences: swap MIN_SQUARE_DISTANCE & MAX_SQUARE_DISTANCE checks
         */
        private static long ProfileFindClosestObject_MinSquareDistanceWithConstraints2 (Transform[] objects, Vector3 center) {
            stopwatch.Restart();
            int closestId           = 0;
            float minSquareDistance = GetSquareDistanceBetween(center, objects[closestId].position);
            float squareDistance;
            const float MIN_SQUARE_DISTANCE = TOO_CLOSE_DISTANCE * TOO_CLOSE_DISTANCE;
            const float MAX_SQUARE_DISTANCE = TOO_FAR_DISTANCE * TOO_FAR_DISTANCE;
            for (int i = 1; i < ELEMENTS_COUNT; ++i) {
                squareDistance = GetSquareDistanceBetween(center, objects[i].position);
                if (squareDistance > MAX_SQUARE_DISTANCE) {
                    continue;
                }
                if (squareDistance < MIN_SQUARE_DISTANCE) {
                    closestId = i;
                    break;
                }
                if (minSquareDistance > squareDistance) {
                    minSquareDistance = squareDistance;
                    closestId = i;
                }
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        //----------------------------------------------------------------------
        /**
         * Naive DOD approach
         */
        private static long ProfileFindClosestObject_MinDistanceDOD (Vector3[] positions, Vector3 center) {
            stopwatch.Restart();
            int closestId     = 0;
            float minDistance = GetDistanceBetween(center, positions[closestId]);
            float distance;
            for (int i = 1; i < ELEMENTS_COUNT; ++i) {
                distance = GetDistanceBetween(center, positions[i]);
                if (minDistance > distance) {
                    minDistance = distance;
                    closestId   = i;
                }
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        //----------------------------------------------------------------------
        /**
         * DOD approach with full optimizations
         */
        private static long ProfileFindClosestObject_MinDistanceDODOptimized (Vector3[] positions, Vector3 center) {
            stopwatch.Restart();
            int closestId           = 0;
            float minSquareDistance = GetSquareDistanceBetween(center, positions[closestId]);
            float squareDistance;
            const float MIN_SQUARE_DISTANCE = TOO_CLOSE_DISTANCE * TOO_CLOSE_DISTANCE;
            const float MAX_SQUARE_DISTANCE = TOO_FAR_DISTANCE * TOO_FAR_DISTANCE;
            for (int i = 1; i < ELEMENTS_COUNT; ++i) {
                squareDistance = GetSquareDistanceBetween(center, positions[i]);
                if (squareDistance > MAX_SQUARE_DISTANCE) {
                    continue;
                }
                if (squareDistance < MIN_SQUARE_DISTANCE) {
                    closestId = i;
                    break;
                }
                if (minSquareDistance > squareDistance) {
                    minSquareDistance = squareDistance;
                    closestId   = i;
                }
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        //----------------------------------------------------------------------
        private static Vector3 GetRandom () {
            return new Vector3(
                randomizer.Next(MIN_RANDOM_VALUE, MAX_RANDOM_VALUE) * RANDOM_FACTOR,
                randomizer.Next(MIN_RANDOM_VALUE, MAX_RANDOM_VALUE) * RANDOM_FACTOR,
                randomizer.Next(MIN_RANDOM_VALUE, MAX_RANDOM_VALUE) * RANDOM_FACTOR
            );
        }
        //----------------------------------------------------------------------
        private static float GetDistanceBetween (Vector3 a, Vector3 b) {
            return MathF.Sqrt(
                (a.x - b.x) * (a.x - b.x) + 
                (a.y - b.y) * (a.y - b.y) + 
                (a.z - b.z) * (a.z - b.z)
            );
        }
        //----------------------------------------------------------------------
        private static float GetSquareDistanceBetween (Vector3 a, Vector3 b) {
            return (
                (a.x - b.x) * (a.x - b.x) + 
                (a.y - b.y) * (a.y - b.y) + 
                (a.z - b.z) * (a.z - b.z)
            );
        }
        //----------------------------------------------------------------------
        #pragma warning disable CS0169, CS0649 // about unused variables
        public struct Vector3 {
            public float x;
            public float y;
            public float z;
            //------------------------------------------------------------------
            public Vector3 (float x, float y, float z) {
                this.x = x;
                this.y = y;
                this.z = z;
            }
            //------------------------------------------------------------------
            public static Vector3 zero () {
                return new Vector3(0f, 0f, 0f);
            }
        }
        //----------------------------------------------------------------------
        public struct Transform {
            public int id;
            public Vector3 position;
            public Vector3 unusedData1;
            public Vector3 unusedData2;
            public Vector3 unusedData3;
            public Vector3 unusedData4;
            public Vector3 unusedData5;
            public Vector3 unusedData6;
            public Vector3 unusedData7;
            public Vector3 unusedData8;
            //------------------------------------------------------------------
            public Transform (int id, Vector3 position) {
                this.id       = id;
                this.position = position;
                unusedData1   = Vector3.zero();
                unusedData2   = Vector3.zero();
                unusedData3   = Vector3.zero();
                unusedData4   = Vector3.zero();
                unusedData5   = Vector3.zero();
                unusedData6   = Vector3.zero();
                unusedData7   = Vector3.zero();
                unusedData8   = Vector3.zero();
            }
        }
        #pragma warning restore CS0169, CS0649
    }
}
