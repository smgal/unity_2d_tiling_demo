
using System;

public class Player
{
	private nd.type.Pos _pos;
	private nd.type.Dir _faced;

	public Player()
	{
		_pos.x = 10;
		_pos.y = 10;
		_faced.dx = 0;
		_faced.dy = 1;
	}

	public nd.type.Dir Diretion
	{
		get { return _faced; }
	}

	public void SetDiretion(int dx, int dy)
	{
		_faced.dx = dx;
		_faced.dy = dy;
	}

	public void SetDiretion(nd.type.Dir faced)
	{
		this.SetDiretion(faced.dx, faced.dy);
	}

	public nd.type.Pos Pos
	{
		get { return _pos; }
	}

	public void Move(int dx, int dy)
	{
		_pos.x += dx;
		_pos.y += dy;
	}

	public void Move(nd.type.Dir dir)
	{
		this.Move(dir.dx, dir.dy);
	}

	public void Warp(int x, int y)
	{
		_pos.x = x;
		_pos.y = y;
	}
}
