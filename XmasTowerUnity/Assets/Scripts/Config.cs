namespace Assets.Scripts
{
    public static class Config
    {
        // Gifts
        public static readonly float MIN_GIFT_SCALE_SIZE = 1f;
        public static readonly float MAX_GIFT_SCALE_SIZE = 5f;

        // Camera
        public static readonly float CAMERA_INTERPOLATION_THRESHOLD = 0.1f;
        public static readonly float CAMERA_INTERPOLATION_SMOOTHNESS = 0.05f;

        // Score
        public static readonly float SCORE_FACTOR = 5f; // Trying to have a consistent value between tower height and centimeters
    }
}
