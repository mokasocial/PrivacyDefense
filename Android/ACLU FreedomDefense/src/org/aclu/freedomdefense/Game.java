package org.aclu.freedomdefense;

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

public class Game implements ApplicationListener {
	private static int screenWidth = 480;
	private static int screenHeight = 320;

	private SpriteBatch batch;
	private Texture spriteSheet;
	private Texture mapData;
	private BitmapFont mFont;
	private int[][] tiles; // Our base map (paths and whatnot)
	private char[][] movementDirs; // Our pathfinding, 'N' 'E' 'W' or 'S' (and
									// can make different for flyers, woah!)
	private int money;
	private int life;
	public Creep[] creeps;
	public Projectile[] projectiles;
	public Tower[] towers;

	@Override
	public void create() {
		batch = new SpriteBatch();
		spriteSheet = new Texture(Gdx.files.internal("sprite_sheet.png"));
		Pixmap mapData = new Pixmap(Gdx.files.internal("map.png"));

		tiles = new int[30][20];
		movementDirs = new char[30][20];
		mFont = new BitmapFont(Gdx.files.internal("ostrich_sans_mellow.fnt"), Gdx.files.internal("ostrich_sans_mellow.png"), false);
		mFont.setFixedWidthGlyphs("LifeMoney0123456789");

		money = 100;
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
			}
		}

		projectiles = new Projectile[0];
		creeps = new Creep[0];
		Tower[] towers = new Tower[1];

		Tower testTower = new Tower();
		testTower.x = 3;
		testTower.y = 4;

		towers[0] = testTower;

		mapData.dispose();
	}

	public void update() {
		float dt = Gdx.graphics.getDeltaTime();

		for (Creep creep : creeps) {
			creep.update(dt);
		}
		for (Projectile projectile : projectiles) {
			projectile.update(dt);
		}
		for (Tower tower : towers) {
			tower.update(dt);
		}
	}

	@Override
	public void render() {
		Gdx.gl.glClear(GL10.GL_COLOR_BUFFER_BIT); // clear the screen
		batch.begin();

		// Draw the terrain!
		for (int x = 0; x < 30; ++x) {
			for (int y = 0; y < 20; ++y) {
				batch.draw(spriteSheet, x * 16, y * 16, tiles[x][y] * 16, 0, 16, 16);

				// Temporary, copy pasta
				switch (movementDirs[x][y]) {
				case 'N':
					batch.draw(spriteSheet, x * 16, y * 16, 11 * 16, 0, 16, 16);
					break;
				case 'E':
					batch.draw(spriteSheet, x * 16, y * 16, 12 * 16, 0, 16, 16);
					break;
				case 'W':
					batch.draw(spriteSheet, x * 16, y * 16, 13 * 16, 0, 16, 16);
					break;
				case 'S':
					batch.draw(spriteSheet, x * 16, y * 16, 14 * 16, 0, 16, 16);
					break;
				}
			}
		}

		// Draw the towers!

		// Draw the creeps!

		// Draw the UI! (forgive me)
		// String newline = System.getProperty("line.separator");
		String uiString = "Life: " + life + '\n' + "Money: " + money;

		TextBounds uiBounds = mFont.getMultiLineBounds(uiString);

		TextureRegion blackBox = new TextureRegion(spriteSheet, 0, 2 * 16, 16, 16);

		// Draw a black rectangle behind the text
		batch.draw(blackBox, screenWidth - uiBounds.width, screenHeight - uiBounds.height, uiBounds.width, uiBounds.height);

		mFont.drawWrapped(batch, uiString, screenWidth - uiBounds.width, screenHeight, uiBounds.width);

		batch.end();

		for (Creep creep : creeps) {
		}
		for (Projectile projectile : projectiles) {
		}
		for (Tower tower : towers) {
			batch.draw(spriteSheet, tower.x * 16, tower.y * 16, 14 * 16, 0, 16, 16);
		}

		money++;
	}

	@Override
	public void resize(int width, int height) {

	}

	@Override
	public void pause() {

	}

	@Override
	public void resume() {

	}

	@Override
	public void dispose() {

	}
}