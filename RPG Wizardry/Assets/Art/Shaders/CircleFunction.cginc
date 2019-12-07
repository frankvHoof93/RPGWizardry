// Checks if a position is inside of a circle
inline bool IsInCircle(float2 position, float2 circleMid, float circleRadius)
{
	return length(position - circleMid) <= circleRadius;
}
// Checks if a position is inside of any of the given circles (small batch)
inline bool IsInAnyCircle(float2 position, float2 circles[4], float radii[4], int length)
{
	for (int i = 0; i < length; i++)
		if (IsInCircle(position, circles[i], radii[i]))
			return true;
	return false;
}
// Checks if a position is inside of any of the given circles (large batch)
inline bool IsInAnyCircleLarge(float2 position, float2 circles[64], float radii[64], int length)
{
	for (int i = 0; i < length; i++)
		if (IsInCircle(position, circles[i], radii[i]))
			return true;
	return false;
}