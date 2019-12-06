inline bool IsInCircle(float2 position, float2 circleMid, float circleRadius)
{
	return length(position - circleMid) <= circleRadius;
}