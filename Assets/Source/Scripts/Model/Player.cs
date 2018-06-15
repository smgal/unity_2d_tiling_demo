
using System;

public class Player
{
	private nd.Pos _pos;
	private nd.Dir _faced;

	public Player()
	{
		_pos.x = 25;
		_pos.y = 25;
		_faced.dx = 0;
		_faced.dy = 1;
	}

	public nd.Dir Diretion
	{
		get { return _faced; }
	}

	public void SetDiretion(int dx, int dy)
	{
		_faced.dx = dx;
		_faced.dy = dy;
	}

	public void SetDiretion(nd.Dir faced)
	{
		this.SetDiretion(faced.dx, faced.dy);
	}

	public nd.Pos Pos
	{
		get { return _pos; }
	}

	public void Move(int dx, int dy)
	{
		_pos.x += dx;
		_pos.y += dy;
	}

	public void Move(nd.Dir dir)
	{
		this.Move(dir.dx, dir.dy);
	}
}
