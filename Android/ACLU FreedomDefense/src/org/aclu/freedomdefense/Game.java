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
import com.badlogic.gdx.math.Rectangle;
import com.badlogic.gdx.math.Vector2;

public class Game implements ApplicationListener {
	public static int screenWidth = 480;
	public static int screenHeight = 320;

	public static final int SQUARE_WIDTH = 24;
	
	public static final Rectangle DRD_PAUSE_RECT = new Rectangle(3, 50, 54, 25);
	public static final Rectangle START_RECT = new Rectangle(3, 77, 54, 25);
	public static final Rectangle RESTART_RECT = new Rectangle(3, 77, 54, 25);
	public static final Rectangle SELL_RECT = new Rectangle(3, 77, 54, 25);
	
	public static final float TIME_BETWEEN_WAVES = 3;
	
	public static final int INITIAL_CREEP_SPEED = 32;

	public static int maxProjectiles = 200;


	private SpriteBatch batch;
	public Texture spriteSheet;
	public Texture menuTexture;
	public Texture corporateCreepTexture;
	public Texture defconZeplinTexture;
	public Texture govHeliTexture;
	public Texture tileset32Texture;
	
	private Texture mapData;
	private BitmapFont mFont;
	public int[][] tiles; // Our base map (paths and whatnot)
	public char[][] movementDirs; // Our pathfinding, 'N' 'E' 'W' or 'S' (and
									// can make different for flyers, woah!)
	
	// This is just to reduce the number of temporary objects
	public int oldMoney;
	public int oldLife;
	
	public boolean buildMode = true;
	
	public int money;
	public int life;
	
	String uiString;
	TextBounds uiBounds;
	
	public ArrayList<Creep> creeps;
	public Projectile projectiles[];
	public ArrayList<Tower> towers;
	public ArrayList<TowerType> free_towers;
	public int startingX, startingY;

	public Tower selected = null;
	
	public boolean runningDrd;
	
	public TextureRegion cursorTexture = null;
	
	
	public int waveNumber = 0;
	
	public int endingX;
	public int endingY;
	
	public int current_creep_speed = INITIAL_CREEP_SPEED;
	
	public boolean isPaused = false;

	public String debugtext = "";


	public float wave_wait_timer = 0;
	
	public TowerType cursorState;
	public int cursorLocX = 0;
	public int cursorLocY = 0;

	// Circle sprites for tower ranges
	HashMap<Float, Sprite> rangeSprites = new HashMap<Float, Sprite>();
	public final int maxmoney = 9999;
	public final int uiPanelWidth = 60;

	public static Game instance;
	
	public Rectangle collisionRectA;
	public Rectangle collisionRectB;
	
	// I'm sorry this is getting cruddy, so sleepy
	TextureRegion blackBox;
	TextureRegion tower_region;
	
	TextureRegion selection_region;
	
	Texture selectionImg;
	
	TextureRegion pause_button_region;
	TextureRegion start_button_region;
	TextureRegion restart_button_region;
	TextureRegion sell_button_region;
	

	/**
	 * Create a game object. Set running_android if being called from an android device.
	 * False otherwise.
	 * @param running_android Flag for running android.
	 */
	public Game(final boolean running_android) {
		runningDrd = running_android;
	}
	
	@Override
	public void create() {
		instance = this;
		
		// Our rects for collision detection
		collisionRectA = new Rectangle( 0, 0, 8, 8 );
		collisionRectB = new Rectangle( 0, 0, 8, 8 );
		
		Gdx.input.setInputProcessor(new GameInputProcessor());

		batch = new SpriteBatch();
		spriteSheet = new Texture(Gdx.files.internal("sprite_sheet.png"));
		selectionImg = new Texture(Gdx.files.internal("Selector32.png"));
		menuTexture = new Texture(Gdx.files.internal("menu2.png"));
		corporateCreepTexture = new Texture(Gdx.files.internal("CorporateCreep32x32.png"));
		defconZeplinTexture = new Texture(Gdx.files.internal("DefconZeppelin.png"));
		govHeliTexture = new Texture(Gdx.files.internal("GovSearcherHeli.png"));
		tileset32Texture = new Texture(Gdx.files.internal("Tileset32.png"));
		
		Pixmap mapData = new Pixmap(Gdx.files.internal("map2.png"));

		creeps = new ArrayList<Creep>();
		projectiles = new Projectile[maxProjectiles];
		
		for( int i = 0; i < maxProjectiles; ++i )
		{
			projectiles[i] = new Projectile( new Vector2( 0.0f, 0.0f ), new Vector2( 0.0f, 0.0f ), 0.0f, "test");
			projectiles[i].active = false;
		}

		towers = new ArrayList<Tower>();

		free_towers = new ArrayList<TowerType>();
		free_towers.add(TowerType.JUDGE);
		free_towers.add(TowerType.FIREWALL);
		free_towers.add(TowerType.TEACHER);
		//free_towers.add(TowerType.LAWSUIT);
		
		cursorState = null;

		tiles = new int[30][20];
		movementDirs = new char[30][20];

		mFont = new BitmapFont(Gdx.files.internal("ostrich_sans_mellow.fnt"), Gdx.files.internal("ostrich_sans_mellow.png"), false);
		mFont.setFixedWidthGlyphs("LifeMoney0123456789");

		money = 200;
		life = 100;

		// Feel free to change this, it is confusing!
		// Movement data is in the GREEN channel of the map:
		HashMap<Integer, Character> cToMoveDir = new HashMap<Integer, Character>();
		cToMoveDir.put(132, 'S');
		cToMoveDir.put(164, 'E');
		cToMoveDir.put(196, 'N');
		cToMoveDir.put(228, 'W');

		// Tile data is in the RED channel of the map:
		HashMap<Integer, Integer> cToTileNumber = new HashMap<Integer, Integer>();
		cToTileNumber.put(0, 4);
		cToTileNumber.put(32, 5);
		cToTileNumber.put(64, 8);
		cToTileNumber.put(96, 6);
		cToTileNumber.put(128, 7);
		cToTileNumber.put(160, 9);
		cToTileNumber.put(192, 10);

		for (int x = 0; x < 30; ++x) {
			for (int y = 0; y < 20; ++y) {
				int col = mapData.getPixel(x, y);

				int r = (col & 0xFF000000) >>> 24;
				int g = (col & 0x00FF0000) >>> 16;
				int b = (col & 0x0000FF00) >>> 8; // Currently have 100 for 1
													// below enemy spawn (so
													// they spawn offscreen),
													// 255 player base

				if (cToTileNumber.containsKey(r))
					tiles[x][19 - y] = cToTileNumber.get(r); // Flip the Y so
																// what we're
																// photoshopping
																// matches the
																// game
				else
					tiles[x][19 - y] = 4;

				if (cToMoveDir.containsKey(g))
					movementDirs[x][19 - y] = cToMoveDir.get(g);
				else
					movementDirs[x][19 - y] = 'S';

				if (b == 100) {
					startingX = x;
					startingY = 19 - y;
				} else if (b == 255) {
					endingX = x;
					endingY = 19 - y;
				}
			}
		}

		for( int i = 1; i < 50; ++i )
			creeps.add( new Creep( 100, current_creep_speed, 20, startingX, startingY + i, 0, 0, CreepType.PETTY ) );

		blackBox = new TextureRegion(spriteSheet, 0, 2 * 16, 16, 16);
		
		tower_region = new TextureRegion( spriteSheet, 0, 0, 16, 16 );
		
		oldMoney = 0;
		oldLife = 0;
		
		uiString = "";
		uiBounds = new TextBounds();
		pause_button_region = new TextureRegion( spriteSheet, 0, 0, 16, 16);
		start_button_region = new TextureRegion( spriteSheet, 0, 0, 16, 16);
		restart_button_region = new TextureRegion( spriteSheet, 0, 0, 16, 16);
		selection_region = new TextureRegion( selectionImg, 0, 0, SQUARE_WIDTH, SQUARE_WIDTH);
		sell_button_region = new TextureRegion(spriteSheet, 0, 0, 16, 16 );
		
		mapData.dispose();
	}


	public void update() {
		float dt = Gdx.graphics.getDeltaTime();

		if (buildMode) {
			return;
		}
		
		if (!isPaused && life > 0) {
			
			for (Projectile projectile : projectiles) 
			{
				if( projectile.active && projectile.my_coords.x > 0 && projectile.my_coords.y > 0 && projectile.my_coords.x * 16 < screenWidth && projectile.my_coords.y * 16 < screenHeight )
				{
					collisionRectA.x =  ( projectile.my_coords.x * 16 ) + 8;
					collisionRectA.y = ( projectile.my_coords.y * 16 ) + 8;
					
					for( int i = 0; i < creeps.size(); ++i )
					{
						if( creeps.get(i).active )
						{
							collisionRectB.x = ( creeps.get(i).x * 16 + creeps.get(i).xOffset ) + 8;
							collisionRectB.y = ( creeps.get(i).y * 16 + creeps.get(i).yOffset ) + 8;
							
							if( collisionRectA.overlaps( collisionRectB ) )
							{
								creeps.get(i).Health -= projectile.damage;
								projectile.active = false;
								break;
							}
						}
					}
				}
				else
				{
					projectile.active = false;
				}
				
				if( projectile.active )
				{
					projectile.update(dt);
				}
			}

	
			int active_creeps = 0;
			
			// Handle projectile collision, creep update, creep death
			for( int i = 0; i < creeps.size(); ++i )
			{
				if( creeps.get(i).active )
				{
					if( creeps.get(i).Health > 0 )
					{
						creeps.get(i).update( dt );
						active_creeps++;
					}
					else
					{
						creeps.get(i).die();
					}
				}
			}

			for( int i = 0; i < towers.size(); ++i ) 
			{
				towers.get(i).update(dt);
			}
		
			if (active_creeps <= 0 && wave_wait_timer < TIME_BETWEEN_WAVES) {
			
				wave_wait_timer += dt;
			} else if (active_creeps <= 0 && wave_wait_timer >= TIME_BETWEEN_WAVES) {
				waveNumber++;
				nextWave(current_creep_speed += 10);
				wave_wait_timer = 0;
			}
		}
	}
		


	@Override
	public void render() {
		// LibGDX you are a MY-STERY TO ME
		update();

		Gdx.gl.glClear(GL10.GL_COLOR_BUFFER_BIT); // clear the screen
		batch.begin();

		TextureRegion tmpRegion = null;
		
		// Draw the terrain!
		for (int x = 0; x < 30; ++x) {
			for (int y = 0; y < 20; ++y) {
				tmpRegion = new TextureRegion( tileset32Texture, (tiles[x][y] - 4) * 32, 0, 32, 32);
				//pause_button_region = new TextureRegion( spriteSheet, tiles[x][y] - 4, 0, 32, 32);
				// switch to 32x32 sprite
				//batch.draw(spriteSheet, x * 16, y * 16, tiles[x][y] * 16, 0, 16, 16);
				batch.draw(tmpRegion, x * SQUARE_WIDTH, y * SQUARE_WIDTH, SQUARE_WIDTH, SQUARE_WIDTH);

				// Temporary, copy pasta
				/*
				 * switch( movementDirs[x][y] ) { case 'N': batch.draw(
				 * spriteSheet, x*16, y*16, 11*16, 0, 16, 16 ); break; case 'E':
				 * batch.draw( spriteSheet, x*16, y*16, 12*16, 0, 16, 16 );
				 * break; case 'W': batch.draw( spriteSheet, x*16, y*16, 13*16,
				 * 0, 16, 16 ); break; case 'S': batch.draw( spriteSheet, x*16,
				 * y*16, 14*16, 0, 16, 16 ); break; }
				 */
				
			}
		}

		// Draw the towers
		for( int i = 0; i < towers.size(); ++i )
		{
			batch.draw(towers.get(i).m_type.getTextureRegion(), towers.get(i).m_x * SQUARE_WIDTH, (towers.get(i).m_y) * SQUARE_WIDTH, SQUARE_WIDTH, SQUARE_WIDTH);
			//drawSprite(towers.get( i ).getIconNum(), towers.get(i).m_x, towers.get(i).m_y);
			if (towers.get(i).selected) {
				//batch.draw(spriteSheet, towers.get(i).m_x * SQUARE_WIDTH, towers.get(i).m_y * SQUARE_WIDTH, SQUARE_WIDTH, SQUARE_WIDTH, towers.get(i).m_type.getSpriteLocX(), towers.get(i).m_type.getSpriteLocY(), 16, 16, false, true);
				//drawSprite(towers.get( i ).getIconNum(), towers.get(i).m_x, towers.get(i).m_y);
				batch.draw(selectionImg, towers.get(i).m_x * SQUARE_WIDTH, towers.get(i).m_y * SQUARE_WIDTH, SQUARE_WIDTH, SQUARE_WIDTH, 0, 0, 32, 32, false, true);
			}

			
		}

		// Draw the creeps!
		for( int i = 0; i < creeps.size(); ++i)
		{

			if( creeps.get(i).active ) {
				TextureRegion toUse = null;
				//TextureRegion toUse = new TextureRegion(corporateCreepTexture);
				if (creeps.get(i).m_type == CreepType.GLOBAL_CORP) {
					toUse = new TextureRegion(corporateCreepTexture);
				} else if (creeps.get(i).m_type == CreepType.SEARCHER) {
					toUse = new TextureRegion(defconZeplinTexture);
				} else if (creeps.get(i).m_type == CreepType.GOVERNMENT) {
					toUse = new TextureRegion(govHeliTexture);
				} else {
					// TODO: Change this to difference art.
					toUse = new TextureRegion(corporateCreepTexture);
				}
				batch.draw(toUse, creeps.get(i).x * SQUARE_WIDTH + creeps.get(i).xOffset, creeps.get(i).y * SQUARE_WIDTH + creeps.get(i).yOffset, SQUARE_WIDTH, SQUARE_WIDTH);
				//batch.draw(toUse, creeps.get(i).x * SQUARE_WIDTH + creeps.get(i).xOffset, creeps.get(i).y * SQUARE_WIDTH + creeps.get(i).yOffset, SQUARE_WIDTH, SQUARE_WIDTH, 0, 32, 32, 32, false, false);
			}
		}

		// Draw the projectiles!
		for( int i = 0; i < maxProjectiles; ++i )
		{
			if( projectiles[i].active )
			{
				// TODO: Change which sprite the projectile uses based on something in the projectile
				batch.draw( spriteSheet, projectiles[i].my_coords.x*SQUARE_WIDTH, projectiles[i].my_coords.y*SQUARE_WIDTH, SQUARE_WIDTH, SQUARE_WIDTH, 0, 16*3, 16, 16, false, false);
			}
		}

		// Draw the UI!

		// Background

		batch.draw(blackBox, 0, 0, uiPanelWidth , screenHeight);

		// Draw the menu bar
		batch.draw(menuTexture,-4,-190);
		//batch.draw(menu_region, 0, -100);
		
		// Draw the free towers.
		
		for (int i = 1; i <= 3; i++) 
		{
			if (free_towers.get(i-1) != null) 
			{
				//tower_region.setRegion( free_towers.get(i-1).getSpriteLocX(), free_towers.get(i-1).getSpriteLocY(), 16, 16 );
				batch.draw(free_towers.get(i-1).getTextureRegion(), 29, (screenHeight - 60 * i) + 5, SQUARE_WIDTH, SQUARE_WIDTH);
				//batch.draw(tower_region, 33, screenHeight - 57 * i, 16, 16);
				
				String towerPrice = "$" + free_towers.get(i-1).getPrice();
				TextBounds priceBounds = mFont.getBounds(towerPrice);
				Color oldColor = mFont.getColor();
				mFont.setColor(Color.RED);
				mFont.drawWrapped(batch, towerPrice, 10, 33 + screenHeight - 58*i + priceBounds.height, priceBounds.width);
				mFont.setColor(oldColor);
			}
		}		
		
		// Keep the GC in its cage as long as possible. 
		if( oldMoney != money || oldLife != life )
		{
			uiString = "+: " + life + '\n' + "$: " + money;
			uiBounds = mFont.getMultiLineBounds(uiString);
		}
		oldMoney = money;
		oldLife = life;
				
		// Draw droid pause button if on an android device.
		if (runningDrd) {
			
			pause_button_region.setRegion(7*17, 23, 2, 2);
			batch.draw(pause_button_region, DRD_PAUSE_RECT.x, DRD_PAUSE_RECT.y, DRD_PAUSE_RECT.width, DRD_PAUSE_RECT.height);
			
			String pause_button_string = "Pause";
			TextBounds pauseButtonBounds = mFont.getBounds(pause_button_string);
			mFont.drawWrapped(batch, pause_button_string,
							(32 - (pauseButtonBounds.width / 2)),
							DRD_PAUSE_RECT.y + pauseButtonBounds.height + 4, pauseButtonBounds.width);
		}
		
		// Draw the restart button if paused or game over.
		if (life <= 0 || isPaused) {
			
			restart_button_region.setRegion(7*17, 23, 2, 2);
			batch.draw(restart_button_region, RESTART_RECT.x, RESTART_RECT.y, RESTART_RECT.width, RESTART_RECT.height);
			
			String restart_button_string = "Restart";
			TextBounds restartButtonBounds = mFont.getBounds(restart_button_string);
			mFont.drawWrapped(batch, restart_button_string,
							(32 - (restartButtonBounds.width / 2)),
							RESTART_RECT.y + restartButtonBounds.height + 4, restartButtonBounds.width);
			
			
		}
		
		// Draw the start button if we are in build mode.
		if (buildMode) {
			
			start_button_region.setRegion(7*17, 23, 2, 2);
			batch.draw(start_button_region, START_RECT.x, START_RECT.y, START_RECT.width, START_RECT.height);
			
			String start_button_string = "Start";
			TextBounds startButtonBounds = mFont.getBounds(start_button_string);
			mFont.drawWrapped(batch, start_button_string,
							(32 - (startButtonBounds.width / 2)),
							START_RECT.y + startButtonBounds.height + 4, startButtonBounds.width);
			
			
		}
		
		// Draw the sell button if we are not paused or in build mode and a tower is selected.
		if (!isPaused && !buildMode && selected != null) {
			
			sell_button_region.setRegion(7*17, 23, 2, 2);
			batch.draw(sell_button_region, SELL_RECT.x, SELL_RECT.y, SELL_RECT.width, SELL_RECT.height);
			
			String sell_button_string = "Sell";
			TextBounds sellButtonBounds = mFont.getBounds(sell_button_string);
			mFont.drawWrapped(batch, sell_button_string,
							(32 - (sellButtonBounds.width / 2)),
							START_RECT.y + sellButtonBounds.height + 4, sellButtonBounds.width);
		}
		
		// Text
		String uiString = "+  " + life + '\n' + "$  " + money;
		TextBounds uiBounds = mFont.getMultiLineBounds(uiString);
		Color oldColor = mFont.getColor();
		mFont.setColor(Color.RED);
		mFont.drawWrapped(batch, uiString, 3, uiBounds.height + 3, uiBounds.width);
		mFont.setColor(oldColor);

		// DEBUG TEXT
		//mFont.drawWrapped(batch, debugtext, 60, uiBounds.height + 3, 1000);		

		// Draw the cursorTexture
		if (cursorState != null && cursorTexture != null) {
			batch.draw(cursorTexture, cursorLocX - 8, screenHeight - cursorLocY - 8);
		}
		
		// Render Paused String if needed in bottom right corner.
		String center_string = null;
		if (isPaused) {
			center_string = "PAUSED";
		} else if (life <= 0) {
			center_string = "GAME OVER";
		}
		
		if (center_string != null) {
			TextBounds pausedBounds = mFont.getMultiLineBounds(center_string);
			Color oldColor2 = mFont.getColor();
			mFont.setColor(Color.RED);
			mFont.drawWrapped(batch, center_string,
					(screenWidth / 2) - (pausedBounds.width / 2),
					(screenHeight / 2) - (pausedBounds.height / 2), pausedBounds.width );
			mFont.setColor(oldColor2);
		}
		
		batch.end();
	}

	public void restart(int creep_speed) {
		
		money = 200;
		
		life = 100;
		
		waveNumber = 0;
		
		nextWave(INITIAL_CREEP_SPEED);
		
		towers.clear();
		
		for (int i = 0; i < projectiles.length; i++) {
			projectiles[i].active = false;
		}
		
	}
	
	public void nextWave(int creep_speed) {
		creeps = new ArrayList<Creep>();
		
		// Signal gc
		System.gc();
		
		CreepType.seedWithWave(waveNumber);
		
		for (int i = 0; i < 100; i++) {
			Creep newOne = new Creep( 100, creep_speed, 20, startingX, startingY + i, 0, 0, CreepType.getRandomCreepType() );
			newOne.xOffset = 0;
			newOne.yOffset = 0;
			creeps.add(newOne);
			
		}
	}
	
	public void drawSprite(int iconNum, int x, int y) {
		int rownum = 0;
		while (iconNum > 16) {
			iconNum -= 16;
			rownum++;
		}
		batch.draw(spriteSheet, x * 16, y * 16, iconNum * 16, rownum * 16, 16, 16);
	}

	public void drawSpriteFloatcoords(int iconNum, int x, int y) {
		int rownum = 0;
		while (iconNum > 16) {
			iconNum -= 16;
			rownum++;
		}
		batch.draw(spriteSheet, x, y, iconNum * 16, rownum * 16, 16, 16);
	}

	@Override
	public void resize(int width, int height) {

	}

	@Override
	public void pause() {
		if (life > 0) {
			isPaused = !isPaused;
		}
	}

	@Override
	public void resume() {

	}

	@Override
	public void dispose() {

	}

	public void drawCircle(float radius, Color color, Vector2 position) {
		if (rangeSprites.containsKey(radius)) {
			Sprite rangedSprite = rangeSprites.get(radius);

			rangedSprite.setPosition(position.x - rangedSprite.getWidth() / 2, position.y - rangedSprite.getHeight() / 2);

			rangedSprite.draw(batch);

			return;
		}

		// The Pixmap's width+height needs to be a power of 2, so
		// calculate the next power of 2 based on the tower Range
		// * 2 (tower range goes both ways)
		int powof2 = 1;
		int shotRange = (int) Math.ceil(radius * 2);
		while (powof2 < shotRange) {
			powof2 <<= 1;
		}

		// Create a new pixmap with the appropriate size
		Pixmap p = new Pixmap(powof2, powof2, Pixmap.Format.RGBA8888);
		p.setColor(color);

		// Tell the pixmap to create a circle with the middle point
		// shotRange / 2 (x and y), and a radius of shotRange / 2
		p.fillCircle(powof2 / 2, powof2 / 2, (int) radius);

		// Make a texture out of the pixmap, and use that to create
		// a Sprite
		Sprite uiTowerRangeCircle = new Sprite(new Texture(p));

		// We can now dispose the pixmap to free up its resources
		p.dispose();

		rangeSprites.put(radius, uiTowerRangeCircle);

		// Set the coordinates and draw the sprite
		uiTowerRangeCircle.setPosition(position.x - powof2 / 2, position.y - powof2 / 2);
		uiTowerRangeCircle.draw(batch);
	}
}