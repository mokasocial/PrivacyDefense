package org.aclu.freedomdefense;

import java.util.ArrayList;
import java.util.HashMap;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.GL10;
import com.badlogic.gdx.graphics.Pixmap;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.BitmapFont.TextBounds;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.g2d.TextureRegion;


public class Game implements ApplicationListener
{
	public static int screenWidth = 480;
	public static int screenHeight = 320;
	
	private SpriteBatch batch;
	private Texture spriteSheet;
	private Texture mapData;
	private BitmapFont mFont;
	private int[][] tiles;			// Our base map (paths and whatnot)
	public char[][] movementDirs;   // Our pathfinding, 'N' 'E' 'W' or 'S' (and can make different for flyers, woah!)
	private int money;
	private int life;
	public ArrayList<Creep> creeps;
	public ArrayList<Projectile> projectiles;
	public ArrayList<Tower> towers;
	int startingX, startingY;
	
	public static Game instance;
	
	@Override
	public void create()
	{
		instance = this;
		
		batch = new SpriteBatch();
		spriteSheet = new Texture( Gdx.files.internal( "sprite_sheet.png" ));
		Pixmap mapData = new Pixmap( Gdx.files.internal( "map.png" ) );
		
		creeps = new ArrayList<Creep>();
		projectiles = new ArrayList<Projectile>();
		towers = new ArrayList<Tower>();
		
		tiles = new int[30][20];
		movementDirs = new char[30][20];
		
		
		mFont = new BitmapFont(Gdx.files.internal( "ostrich_sans_mellow.fnt" ), Gdx.files.internal( "ostrich_sans_mellow.png" ), false );
		mFont.setFixedWidthGlyphs("LifeMoney0123456789");
		
		money = 100;
		life = 100;
		
		// Feel free to change this, it is confusing!
		// Movement data is in the GREEN channel of the map:
		HashMap<Integer,Character> cToMoveDir = new HashMap<Integer,Character>();
		cToMoveDir.put( 132, 'S' );
		cToMoveDir.put( 164, 'E' );
		cToMoveDir.put( 196, 'N' );
		cToMoveDir.put( 228, 'W' );
		
		// Tile data is in the RED channel of the map:
		HashMap<Integer,Integer> cToTileNumber = new HashMap<Integer,Integer>();
		cToTileNumber.put( 0, 4 );
		cToTileNumber.put( 32, 5 );
		cToTileNumber.put( 64, 8 );
		cToTileNumber.put( 96, 6 );
		cToTileNumber.put( 128, 7 );
		cToTileNumber.put( 160, 9 );
		cToTileNumber.put( 192, 10 );
		
		for( int x = 0; x < 30; ++x )
		{
			for( int y = 0; y < 20; ++y )
			{
				int col = mapData.getPixel( x, y );
				
				int r = ( col & 0xFF000000 ) >>> 24;
				int g = ( col & 0x00FF0000 ) >>> 16;
				int b = ( col & 0x0000FF00 ) >>> 8; // Currently have 100 for 1 below enemy spawn (so they spawn offscreen), 255 player base
			
				if( cToTileNumber.containsKey( r ) )
					tiles[x][19-y] = cToTileNumber.get( r ); // Flip the Y so what we're photoshopping matches the game
				else
					tiles[x][19-y] = 4;
				
				if( cToMoveDir.containsKey( g ) )
					movementDirs[x][19-y] = cToMoveDir.get( g );
				else
					movementDirs[x][19-y] = 'S';
				
				if( b == 100 )
				{
					startingX = x;
					startingY = 19-y;
				}
			}
		}
		
		mapData.dispose();
	}
	
	public void update()
	{
		float dt = Gdx.graphics.getDeltaTime();
		
		for (Creep creep : creeps) {
			creep.update( dt );
		}
		for (Projectile projectile : projectiles) {
			projectile.update( dt );
		}
		for (Tower tower : towers) {
			tower.update( dt );
		}
		
		// Add creeps!
		creeps.add( new Creep( 100, 32, 20, startingX, startingY, 0, 15, CreepType.PETTY ) );
	}
	
	@Override
	public void render()
	{
		// uhh
		update();
		
		Gdx.gl.glClear( GL10.GL_COLOR_BUFFER_BIT ); // clear the screen
		batch.begin();
		
		// Draw the terrain!
		for( int x = 0; x < 30; ++x )
		{
			for( int y = 0; y < 20; ++y )
			{
				batch.draw( spriteSheet, x*16, y*16, tiles[x][y]*16, 0, 16, 16 );

				// Temporary, copy pasta
				/*switch( movementDirs[x][y] )
				{
				case 'N':
					batch.draw( spriteSheet, x*16, y*16, 11*16, 0, 16, 16 );
					break;
				case 'E':
					batch.draw( spriteSheet, x*16, y*16, 12*16, 0, 16, 16 );
					break;
				case 'W':
					batch.draw( spriteSheet, x*16, y*16, 13*16, 0, 16, 16 );
					break;
				case 'S':
					batch.draw( spriteSheet, x*16, y*16, 14*16, 0, 16, 16 );
					break;
				}*/
			}
		}
		
		// Draw the towers
		for (Tower tower : towers) {
			drawSprite(tower.getIconNum(), tower.m_x, tower.m_y);
		}
		
		// Draw the creeps!
		for( Creep creep : creeps )
		{
			batch.draw( spriteSheet, creep.x * 16 + creep.xOffset, creep.y * 16 + creep.yOffset, 0, 16, 16, 16 );
		}
		
		// Draw the UI! (forgive me)
		//String newline = System.getProperty("line.separator");
		String uiString = "Life: " + life + '\n' + "Money: " + money;
		
		TextBounds uiBounds = mFont.getMultiLineBounds( uiString );
		
		TextureRegion blackBox = new TextureRegion( spriteSheet, 0, 2 * 16, 16, 16 );
		
		// Draw a black rectangle behind the text
		batch.draw( blackBox, screenWidth - uiBounds.width, screenHeight - uiBounds.height, uiBounds.width, uiBounds.height );
				
		mFont.drawWrapped( batch, uiString, screenWidth - uiBounds.width, screenHeight, uiBounds.width );
		
		batch.end();
		
		money++;
	}
	public void drawSprite(int iconNum, int x, int y) {
		batch.draw( spriteSheet, x*16, y*16, iconNum*16, 0, 16, 16 );
	}
	
	@Override
	public void resize( int width, int height )
	{
		
	}
	
	@Override
	public void pause()
	{
		
	}
	
	@Override
	public void resume()
	{
		
	}
	
	@Override
	public void dispose()
	{
		
	}
}