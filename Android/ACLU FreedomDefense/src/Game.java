import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;

public class Game implements ApplicationListener
{
	private SpriteBatch batch;
	
	public void create()
	{
		batch = new SpriteBatch();
	}
	
	public void render()
	{
		Gdx.gl.glClear( GL10.GL_COLOR_BUFFER_BIT ); // clear the screen
		batch.begin();
		// Drawing goes here!
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
