package org.aclu.freedomdefense;

import java.util.HashMap;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.GL10;
import com.badlogic.gdx.graphics.Pixmap;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;

public class Game implements ApplicationListener {
	private SpriteBatch batch;
	private Texture spriteSheet;
	private Texture mapData;
	private int[][] tiles; // Our base map (paths and whatnot)
	private char[][] movementDirs; // Our pathfinding, 'N' 'E' 'W' or 'S' (and
									// can make different for flyers, woah!)

	public Creep[] creeps;
	public Projectile[] projectiles;
	public Tower[] towers;

	@Override
	public void create() {
		batch = new SpriteBatch();
		spriteSheet = new Texture(Gdx.files.internal("sprite_sheet.png"));
		tiles = new int[30][20];
		movementDirs = new char[30][20];
		Pixmap mapData = new Pixmap(Gdx.files.internal("map.png"));

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
		
		towers = Tower[2]();
		
		Tower testTower = new Tower();
		testTower.x = 3;
		testTower.y = 4;

		towers[0] = testTower;
		
		mapData.dispose();
	}

	public void update() {
		for (Creep creep : creeps) {
			creep.update();
		}
		for (Projectile projectile : projectiles) {
			projectile.update();
		}
		for (Tower tower : towers) {
			tower.update();
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
		batch.end();

		for (Creep creep : creeps) {
		}
		for (Projectile projectile : projectiles) {
		}
		for (Tower tower : towers) {
			batch.draw(spriteSheet, tower.x * 16, tower.y * 16, 14 * 16, 0, 16, 16);
		}
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