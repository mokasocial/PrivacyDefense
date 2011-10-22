package org.aclu.freedomdefense;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.GL10;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;


public class Game implements ApplicationListener
{
	private SpriteBatch batch;
	private Texture spriteSheet;
	private int[][] tiles;			// Our base map (paths and whatnot)
	private int[][] movementDirs;   // Our pathfinding, each tile is seriously just a value from 0 to 3 (N,E,W,S) of the direction to move until the creep hits the next tile
	
	public void create()
	{
		batch = new SpriteBatch();
		spriteSheet = new Texture( Gdx.files.internal( "sprite_sheet.png" ));
		tiles = new int[30][20];
		
		for( int x = 0; x < 30; ++x )
			for( int y = 0; y < 20; ++y )
				tiles[x][y] = 4;
		
		for( int y = 19; y >= 0; y-- )
			tiles[1][y] = 5;
		
		tiles[1][0] = 8;
		
		for( int x = 2; x < 6; ++x )
		{
			tiles[x][0] = 6;
		}
		
		tiles[5][0] = 7;
		
		for( int y = 1; y < 18; ++y )
		{
			tiles[5][y] = 5;
		}
	}
	
	public void render()
	{
		Gdx.gl.glClear( GL10.GL_COLOR_BUFFER_BIT ); // clear the screen
		batch.begin();
		// Draw the terrain!
		for( int x = 0; x < 30; ++x )
		{
			for( int y = 0; y < 20; ++y )
			{
				batch.draw( spriteSheet, x*16, y*16, tiles[x][y]*16, 0, 16, 16 );
			}
		}
		batch.end();
	}
	
	public void resize( int width, int height )
	{
		
	}
	
	public void pause()
	{
		
	}
	
	public void resume()
	{
		
	}
	
	public void dispose()
	{
		
	}
}