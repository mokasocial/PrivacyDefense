package org.aclu.freedomdefense;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.TextureRegion;
import com.badlogic.gdx.math.Vector2;

public class TowerType {

	private final String m_towerType;
	private final int m_price;

	private Texture m_texture = null;
	private TextureRegion textureRegion = null;

	private TowerType(String type, int price, Texture usableTexture) {
		m_towerType = type;
		m_price = price;
		m_texture = usableTexture;
	}

	public int getPrice() {
		return m_price;
	}

	public TextureRegion getTextureRegion() {
		if (textureRegion == null) {
			textureRegion = new TextureRegion(m_texture, 0, 0, 32, 32);
		}
		return textureRegion;
	}
	
	public static TowerType JUDGE = new TowerType("JUDGE", 50, new Texture(Gdx.files.internal("Judge.png")));
	public static TowerType FIREWALL = new TowerType("FIREWALL", 100, new Texture(Gdx.files.internal("LawyerTower.png")));
	public static TowerType TEACHER = new TowerType("TEACHER", 200, new Texture(Gdx.files.internal("Teacher.png")));
	//public static TowerType LAWSUIT = new TowerType("LAWSUIT", 300, new Vector2( 8*16, 1*16));
	
	public static TowerType[] Values = {
		JUDGE,
		FIREWALL,
		TEACHER,
		//LAWSUIT
	};	
	
}