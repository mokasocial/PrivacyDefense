package org.aclu.freedomdefense;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Input;
import com.badlogic.gdx.Input.Buttons;
import com.badlogic.gdx.InputProcessor;
import com.badlogic.gdx.graphics.g2d.TextureRegion;

public class GameInputProcessor implements InputProcessor {

	public static final int DEFAULT_WIN_PAUSE_KEY = Input.Keys.SPACE;
	public static final int DEFAULT_DRD_PAUSE_KEY = Input.Keys.BACK;

	public GameInputProcessor() {
		Gdx.input.setInputProcessor(this);
		// other constructor stuff here...
	}

	@Override
	public boolean keyDown(int keycode) {
		return false;
	}

	@Override
	public boolean keyTyped(char character) {
		// Game.instance.debugtext = "key typed: '" + character + "'";
		return false;
	}

	@Override
	public boolean keyUp(int keycode) {
		// Game.instance.debugtext = "key up: " + keycode;
		if ((keycode == DEFAULT_WIN_PAUSE_KEY || keycode == DEFAULT_DRD_PAUSE_KEY) && !Game.instance.buildMode) {
			Game.instance.pause();
		}
		return false;
	}

	@Override
	public boolean touchDown(int x, int y, int pointer, int button) {
		// we only want single / left clicks
		if (pointer != 0 || button != Buttons.LEFT) {
			return false;
		}

		Game.instance.debugtext = "touch down: " + x + ", " + y + ", button: " + getButtonString(button);

		if (Game.instance.buildMode) {
			
			if (x >=  Game.START_RECT.x && x <= Game.START_RECT.x + Game.START_RECT.width &&
					(Game.screenHeight - y) >=  Game.START_RECT.y && (Game.screenHeight - y) <= Game.START_RECT.y + Game.START_RECT.height) {
				Game.instance.debugtext = "touch down on START rect";
				Game.instance.buildMode = false;
				Game.instance.cursorLocX = x;
				Game.instance.cursorLocY = y;
				return false;
			}
			
		}
		
		if (Game.instance.isPaused) {
			
			if (x >=  Game.RESTART_RECT.x && x <= Game.RESTART_RECT.x + Game.RESTART_RECT.width &&
					(Game.screenHeight - y) >=  Game.RESTART_RECT.y && (Game.screenHeight - y) <= Game.START_RECT.y + Game.RESTART_RECT.height) {
				Game.instance.debugtext = "touch down on RESTART rect";
				Game.instance.restart(Game.INITIAL_CREEP_SPEED);
				Game.instance.buildMode = true;
				Game.instance.isPaused = false;
				Game.instance.cursorLocX = x;
				Game.instance.cursorLocY = y;
				return false;
			}

			
		}
		
		// Check for touch down on the pause button.
		if (Game.instance.runningDrd && !Game.instance.buildMode &&
				x >=  Game.DRD_PAUSE_RECT.x && x <= Game.DRD_PAUSE_RECT.x + Game.DRD_PAUSE_RECT.width &&
				(Game.screenHeight - y) >=  Game.DRD_PAUSE_RECT.y && (Game.screenHeight - y) <= Game.DRD_PAUSE_RECT.y + Game.DRD_PAUSE_RECT.height) {
			Game.instance.debugtext = "touch down on pause rect";
			Game.instance.cursorState = null;
			Game.instance.cursorLocX = x;
			Game.instance.cursorLocY = y;
			Game.instance.pause();
			return false;
		}
		
		// Check to see if the user is selecting a tower.
		if (!Game.instance.isPaused && !Game.instance.buildMode) {
			
			for (int i = 0; i < Game.instance.towers.size(); i++) {
				// Calculate the closest available square.
				int gameCoordX = (x ) / Game.instance.SQUARE_WIDTH;
				int gameCoordY = (Game.instance.screenHeight - y) / Game.instance.SQUARE_WIDTH;
				
				if (Game.instance.towers.get(i).m_x == gameCoordX &&
						Game.instance.towers.get(i).m_y == gameCoordY) {
					if (Game.instance.selected != null) {
						Game.instance.selected.selected = false;
					}
					
					Game.instance.towers.get(i).selected = true;
					Game.instance.selected = Game.instance.towers.get(i);
				}
			}
			
			// If there user hits the sell button
		    if (Game.instance.selected != null &&
					 x >=  Game.START_RECT.x && x <= Game.START_RECT.x + Game.START_RECT.width &&
					(Game.screenHeight - y) >=  Game.START_RECT.y && (Game.screenHeight - y) <= Game.START_RECT.y + Game.START_RECT.height) {
		    	
		    	Game.instance.money += Game.instance.selected.m_type.getPrice() / 2;
		    	Game.instance.towers.remove(Game.instance.selected);
		    	Game.instance.selected = null;
		    	
		    }
		}
		
		if (!Game.instance.isPaused) {
		
			for (TowerType towerType : Game.instance.free_towers) {
				if (x <= Game.instance.uiPanelWidth && x >= 35) {
					if (y >= (30) && y <= (60)) {
						Game.instance.debugtext = "touch down on tower 1";
						Game.instance.cursorState = Game.instance.free_towers.get(0);					
						Game.instance.cursorLocX = x;
						Game.instance.cursorLocY = y;
					} else if (y >= (90) && y <= (120)) {
						Game.instance.debugtext = "touch down on tower 2";
						Game.instance.cursorState = Game.instance.free_towers.get(1);
						Game.instance.cursorLocX = x;
						Game.instance.cursorLocY = y;
					} else if (y >= (150) && y <= (180)) {
						Game.instance.debugtext = "touch down on tower 3";
						Game.instance.cursorState = Game.instance.free_towers.get(2);
						Game.instance.cursorLocX = x;
						Game.instance.cursorLocY = y;
					} /*else if (y >= (140) && y <= (160)) {
						Game.instance.debugtext = "touch down on tower 4";
						Game.instance.cursorState = Game.instance.free_towers.get(3);
						Game.instance.cursorLocX = x;
						Game.instance.cursorLocY = y;
					}*/
				
					if (Game.instance.cursorState != null) {
						Game.instance.cursorTexture = new TextureRegion(Game.instance.cursorState.getTextureRegion(),
								0,
								0,
								32, 32);
					}
				}				
			}
		}
		return false;
	}

	@Override
	public boolean touchDragged(int x, int y, int pointer) {
		Game.instance.debugtext = "touch dragged: " + x + ", " + y + ", pointer: " + pointer;

		if (Game.instance.cursorState != null) {
			Game.instance.cursorLocX = x;
			Game.instance.cursorLocY = y;
		}

		return false;
	}

	private boolean tileContainsTower(int x, int y) {
		boolean flag = false;
		for (Tower tower : Game.instance.towers) {
			if (tower.m_x == x && tower.m_y == y) {
				flag = true;
			}
		}
		return flag;
	}

	@Override
	public boolean touchUp(int x, int y, int pointer, int button) {

		// If the cursor state is not null then place a tower of that type in
		// the location
		if (Game.instance.cursorState != null) {

			// Calculate the closest available square.

			int gameCoordX = (x ) / Game.instance.SQUARE_WIDTH;
			int gameCoordY = (Game.instance.screenHeight - y) / Game.instance.SQUARE_WIDTH;
			
			//System.out.println(Game.instance.tiles[gameCoordX][gameCoordY]);
			if (!tileContainsTower(gameCoordX, gameCoordY) &&
					!(Game.instance.tiles[gameCoordX][gameCoordY] >= 5 &&
					  Game.instance.tiles[gameCoordX][gameCoordY] <= 10)) {
				
				// Check to make sure there is enough money to buy the tower.
				if (Game.instance.money >= Game.instance.cursorState.getPrice()) {
					Game.instance.towers.add(new Tower(Game.instance.cursorState, gameCoordX, gameCoordY));
					Game.instance.money -= Game.instance.cursorState.getPrice();
				}
			}
		}

		Game.instance.cursorLocX = 0;
		Game.instance.cursorLocY = 0;
		Game.instance.cursorState = null;

		if (Buttons.LEFT != button || pointer != 0) {
			return false;
		}
		// Game.instance.debugtext = "touch up: " + x + ", " + y + ", button: "
		// + getButtonString(button);

		return false;
	}

	@Override
	public boolean touchMoved(int x, int y) {
		Game.instance.debugtext = "touch moved: " + x + ", " + y;
		return false;
	}

	@Override
	public boolean scrolled(int amount) {
		// Game.instance.debugtext = "scrolled: " + amount;
		return false;
	}

	private String getButtonString(int button) {
		if (button == Buttons.LEFT)
			return "left";
		if (button == Buttons.RIGHT)
			return "right";
		if (button == Buttons.MIDDLE)
			return "middle";
		return "left";
	}
}
