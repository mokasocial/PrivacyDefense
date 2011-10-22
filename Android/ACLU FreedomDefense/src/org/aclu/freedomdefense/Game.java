package org.aclu.freedomdefense;

import java.util.ArrayList;
import java.util.HashMap;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.GL10;
import com.badlogic.gdx.graphics.Pixmap;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.BitmapFont.TextBounds;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.g2d.TextureRegion;
import com.badlogic.gdx.math.Vector2;


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
	public int money;
	public int life;
	public ArrayList<Creep> creeps;
	public ArrayList<Projectile> projectiles;
	public ArrayList<Tower> towers;
	int startingX, startingY;
	int endingX, endingY;
	
	// Circle sprites for tower ranges
	HashMap<Float, Sprite> rangeSprites = new HashMap<Float,Sprite>();
	
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
		
		towers.add(new Tower(TowerType.JUDGE, 6, 6));
		towers.add(new Tower(TowerType.LAWSUIT, 12, 4));
		towers.add(new Tower(TowerType.TEACHER, 20, 8));
		
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
				else if( b == 255 )
				{
					endingX = x;
					endingY = 19-y;
				}
			}
		}
		
		for( int i = 1; i < 100; ++i )
			creeps.add( new Creep( 100, 32, 20, startingX, startingY + i, 0, 0, CreepType.PETTY ) );
		
		mapData.dispose();
	}
	
	public void update()
	{
		float dt = Gdx.graphics.getDeltaTime();
		
		for (Projectile projectile : projectiles) 
		{
			projectile.update( dt );
		}
		
		//ArrayList<Creep> livingCreeps = new ArrayList<Creep>();
		
		for (Creep creep : creeps) 
		{
			if( creep.Health > 0 )
			{
				creep.update( dt );
				//livingCreeps.add( creep );
			}
			else
			{
				creep.die();
			}
		}
		
		//creeps = livingCreeps;
		
		for (Tower tower : towers) 
		{
			tower.update( dt );
		}
	}
	
	@Override
	public void render()
	{
		// LibGDX you are a mystery to me
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
		for (Tower tower : towers) 
		{
			drawSprite(tower.getIconNum(), tower.m_x, tower.m_y);
			
			// Debugging
			//drawCircle( tower.radius, new Color(0.0f, 1.0f, 0.0f, 0.5f), new Vector2( tower.m_x * 16 + 8, tower.m_y * 16 + 8) );
		}
		
		// Draw the creeps!
		for( Creep creep : creeps )
		{
			batch.draw( spriteSheet, creep.x * 16 + creep.xOffset, creep.y * 16 + creep.yOffset, 0, 16, 16, 16 );
		}
		
		// Draw the UI! (forgive me)
		String uiString = "Life: " + life + '\n' + "Money: " + money;
		
		TextBounds uiBounds = mFont.getMultiLineBounds( uiString );
		
		TextureRegion blackBox = new TextureRegion( spriteSheet, 0, 2 * 16, 16, 16 );
		
		// Draw a black rectangle behind the text
		batch.draw( blackBox, screenWidth - uiBounds.width, screenHeight - uiBounds.height, uiBounds.width, uiBounds.height );
				
		mFont.drawWrapped( batch, uiString, screenWidth - uiBounds.width, screenHeight, uiBounds.width );
		
		batch.end();
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
	
	public void drawCircle( float radius, Color color, Vector2 position )
	{
		if( rangeSprites.containsKey( radius  ) )
		{
			Sprite rangedSprite = rangeSprites.get( radius );
			
			rangedSprite.setPosition( position.x - rangedSprite.getWidth()/2, position.y - rangedSprite.getHeight()/2 );
			
			rangedSprite.draw( batch );
			
			return;
		}
		
		// The Pixmap's width+height needs to be a power of 2, so
		// calculate the next power of 2 based on the tower Range
		// * 2 (tower range goes both ways)
		int powof2 = 1;
		int shotRange = (int)Math.ceil( radius * 2 );
		while( powof2 < shotRange) powof2 <<= 1;
		 
		// Create a new pixmap with the appropriate size
		Pixmap p = new Pixmap(powof2, powof2, Pixmap.Format.RGBA8888);
		p.setColor( color );
		
		// Tell the pixmap to create a circle with the middle point
		// shotRange / 2 (x and y), and a radius of shotRange / 2
		p.fillCircle( powof2/2, powof2/2, (int)radius );
		 
		// Make a texture out of the pixmap, and use that to create
		// a Sprite
		Sprite uiTowerRangeCircle = new Sprite(new Texture(p));
		 
		// We can now dispose the pixmap to free up its resources
		p.dispose();
		
		rangeSprites.put( radius, uiTowerRangeCircle );
		
		// Set the coordinates and draw the sprite
		uiTowerRangeCircle.setPosition( position.x - powof2/2, position.y - powof2/2);
		uiTowerRangeCircle.draw( batch );
	}
}