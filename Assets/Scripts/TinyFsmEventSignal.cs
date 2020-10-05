using System;

public enum TinyFsmEventSignal
{
	SUPER = -1,
	INIT = -2,
	ENTER = -3,
	LEAVE = -4,
	UPDATE = 0,
	MESSAGE,
	USER = 100
}
