inline bool IsInCircle(float2 position, float2 circleMid, float circleRadius)
{
	return length(position - circleMid) <= circleRadius;
}

inline bool IsInAnyCircle(float2 position, float2 circles[4], float radii[4], int length)
{
	for (int i = 0; i < length; i++)
	{
		if (IsInCircle(position, circles[i], radii[i]))
		{
			return true;
		}
	}
	return false;
}

inline bool IsInAnyCircleLarge(float2 position, float2 circles[64], float radii[64], int length)
{
	for (int i = 0; i < length; i++)
	{
		if (IsInCircle(position, circles[i], radii[i]))
		{
			return true;
		}
	}
	return false;
}