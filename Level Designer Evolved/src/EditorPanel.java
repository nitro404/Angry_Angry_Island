// ======================================= //
// Level Designer Evolved                  //
//                                         //
// Author: Kevin Scroggins                 //
// E-Mail: nitro404@hotmail.com            //
// ======================================= //

import java.awt.*;
import java.awt.event.*;

import javax.swing.*;

public class EditorPanel extends JPanel implements Scrollable, ActionListener, MouseListener, MouseMotionListener {
	
	private static final long serialVersionUID = 1L;
	
	private Level level;
	
	private EditorWindow editorWindow;
	
	private Point selectedPoint;
	private Vertex selectedVertex;
	private Vertex vertexToMove;
	private Vertex lastSelectedVertex;
	private Entity selectedSprite;
	private Entity spriteToMove;
	
	private JPopupMenu drawingPopupMenu;
	private JMenuItem drawingPopupMenuNewVertex;
	private JMenuItem drawingPopupMenuDeleteVertex;
	private JCheckBox drawingPopupMenuConnectVertices;
	private JMenuItem drawingPopupMenuTileMode;
	private JMenuItem drawingPopupMenuCancel;
	
	private JPopupMenu tilingPopupMenu;
	private JMenuItem tilingPopupMenuBringSpriteToFront;
	private JMenuItem tilingPopupMenuSendSpriteToBack;
	private JMenuItem tilingPopupMenuDeleteSprite;
	private JMenuItem tilingPopupMenuDrawMode;
	private JMenuItem tilingPopupMenuCancel;
	
	private Point selectedGridBlock;
	public int mode;
	public boolean collisionLinesEnabled;
	public boolean gridEnabled;
	final public static int MODE_TILING = 0;
	final public static int MODE_DRAWING = 1;
	final private static int DEFAULT_SELECTION_RADIUS = 6;
	private Point mousePosition;
	
	public static Color DEFAULT_GRID_COLOUR = new Color(153, 153, 153);
	public static Color DEFAULT_LINE_COLOUR = new Color(255, 255, 0);
	public static Color DEFAULT_VERTEX_COLOUR = new Color(255, 102, 0);
	public static Color DEFAULT_SELECTED_COLOUR = new Color(255, 0, 0);
	public static Color DEFAULT_BACKGROUND_COLOUR = new Color(255, 255, 255);
	
	public Color selectedColour;
	public Color gridColour;
	public Color lineColour;
	public Color vertexColour;
	public Color backgroundColour;
	
	final private int doubleClickSpeed = 200;
	private long lastMouseDown = 0;
	
	public EditorPanel(EditorWindow editorWindow, VariableSystem settings) {
		level = null;
		setLayout(null);
		addMouseListener(this);
		addMouseMotionListener(this);
		
		this.editorWindow = editorWindow;
		if(editorWindow == null) {
			System.out.println("ERROR: Editor Window cannot be null.");
			System.exit(1);
		}
		
		createPopupMenus();
		
		mode = MODE_TILING;
		gridEnabled = true;
		collisionLinesEnabled = true;
		selectedGridBlock = null;
		
		selectedPoint = null;
		selectedVertex = null;
		vertexToMove = null;
		lastSelectedVertex = null;
		selectedSprite = null;
		spriteToMove = null;
	
		loadSettings(settings);
		
		this.update();
	}
	
	private void loadSettings(VariableSystem settings) {
		gridColour = DEFAULT_GRID_COLOUR;
		lineColour = DEFAULT_LINE_COLOUR;
		vertexColour = DEFAULT_VERTEX_COLOUR;
		selectedColour = DEFAULT_SELECTED_COLOUR;
		backgroundColour = DEFAULT_BACKGROUND_COLOUR;
		if(settings != null) {
			Color temp;
			if((temp = Utilities.parseColour(settings.getValue("Grid Colour"))) != null) {
				gridColour = temp;
			}
			if((temp = Utilities.parseColour(settings.getValue("Line Colour"))) != null) {
				lineColour = temp;
			}
			if((temp = Utilities.parseColour(settings.getValue("Vertex Colour"))) != null) {
				vertexColour = temp;
			}
			if((temp = Utilities.parseColour(settings.getValue("Selected Colour"))) != null) {
				selectedColour = temp;
			}
		}
	}
	
	private void createPopupMenus() {
		drawingPopupMenu = new JPopupMenu();
		tilingPopupMenu = new JPopupMenu();
		
		drawingPopupMenuNewVertex = new JMenuItem("Create Vertex");
		drawingPopupMenuDeleteVertex = new JMenuItem("Delete Vertex");
		drawingPopupMenuConnectVertices = new JCheckBox("Auto-Connect Points");
		drawingPopupMenuTileMode = new JMenuItem("Texture Tiling Mode");
		drawingPopupMenuCancel = new JMenuItem("Cancel");
		
		tilingPopupMenuBringSpriteToFront = new JMenuItem("Bring Sprite to Front");
		tilingPopupMenuSendSpriteToBack = new JMenuItem("Send Sprite to Back");
		tilingPopupMenuDeleteSprite = new JMenuItem("Delete Sprite");
		tilingPopupMenuDrawMode = new JMenuItem("Boundary Drawing Mode");
		tilingPopupMenuCancel = new JMenuItem("Cancel");
		
		drawingPopupMenuConnectVertices.setSelected(true);
		
		drawingPopupMenuNewVertex.addActionListener(this);
		drawingPopupMenuDeleteVertex.addActionListener(this);
		drawingPopupMenuConnectVertices.addActionListener(this);
		drawingPopupMenuTileMode.addActionListener(this);
		
		tilingPopupMenuBringSpriteToFront.addActionListener(this);
		tilingPopupMenuSendSpriteToBack.addActionListener(this);
		tilingPopupMenuDeleteSprite.addActionListener(this);
		tilingPopupMenuDrawMode.addActionListener(this);
		
		drawingPopupMenu.add(drawingPopupMenuNewVertex);
		drawingPopupMenu.add(drawingPopupMenuDeleteVertex);
		drawingPopupMenu.add(drawingPopupMenuConnectVertices);
		drawingPopupMenu.addSeparator();
		drawingPopupMenu.add(drawingPopupMenuTileMode);
		drawingPopupMenu.add(drawingPopupMenuCancel);
		
		tilingPopupMenu.add(tilingPopupMenuBringSpriteToFront);
		tilingPopupMenu.add(tilingPopupMenuSendSpriteToBack);
		tilingPopupMenu.add(tilingPopupMenuDeleteSprite);
		tilingPopupMenu.addSeparator();
		tilingPopupMenu.add(tilingPopupMenuDrawMode);
		tilingPopupMenu.add(tilingPopupMenuCancel);
	}
	
	public void setLevel(Level level) {
		this.level = level;
	}
	
	public Dimension getPreferredSize() {
		if(level != null) {
			return level.dimensions;
		}
		else {
			return new Dimension(16 * Level.GRID_SIZE, 16 * Level.GRID_SIZE);
		}
	}
	
	public Dimension getPreferredScrollableViewportSize() {
		return getPreferredSize();
	}

	public int getScrollableUnitIncrement(Rectangle visibleRect, int orientation, int direction) {
		int currentPosition = 0;
		if(orientation == SwingConstants.HORIZONTAL) {
			currentPosition = visibleRect.x;
		}
		else {
			currentPosition = visibleRect.y;
		}
        
		int maxUnitIncrement = 7;
		if(direction < 0) {
			int newPosition = currentPosition -
							  (currentPosition / maxUnitIncrement)
                              * maxUnitIncrement;
            return (newPosition == 0) ? maxUnitIncrement : newPosition;
        }
		else {
            return ((currentPosition / maxUnitIncrement) + 1)
                   * maxUnitIncrement
                   - currentPosition;
        }
	}
	
	public int getScrollableBlockIncrement(Rectangle visibleRect, int orientation, int direction) {
		if(orientation == SwingConstants.HORIZONTAL) {
			return visibleRect.width - Level.GRID_SIZE;
		}
		else {
			return visibleRect.height - Level.GRID_SIZE;
		}
	}
	
	public boolean getScrollableTracksViewportHeight() {
		return false;
	}

	public boolean getScrollableTracksViewportWidth() {
		return false;
	}
	
	public void mouseClicked(MouseEvent e) { }
	public void mouseEntered(MouseEvent e) { }
	public void mouseExited(MouseEvent e) { }
	
	public void mousePressed(MouseEvent e) {
		if(e.getButton() == MouseEvent.BUTTON1) {
			if(mode == MODE_DRAWING && e.getWhen() - lastMouseDown < doubleClickSpeed) {
				Vertex newVertex = new Vertex(e.getPoint());
				level.addVertex(newVertex);
				if(drawingPopupMenuConnectVertices.isSelected() && lastSelectedVertex != null) {
					level.addEdge(new Edge(lastSelectedVertex, newVertex));
				}
			}
		}
		else if(e.getButton() == MouseEvent.BUTTON2) {
			if(mode == MODE_DRAWING) {
				vertexToMove = null;
				Vertex previousVertex = selectedVertex;
				
				selectedPoint = e.getPoint();
				selectVertex(e.getPoint(), DEFAULT_SELECTION_RADIUS);
				vertexToMove = selectedVertex;
				
				selectedVertex = previousVertex;
				lastSelectedVertex = selectedVertex;
			}
			else if(mode == MODE_TILING) {
				editorWindow.activeSprite = null;
				vertexToMove = null;
				
				selectSprite(e.getPoint());
				spriteToMove = selectedSprite;
			}
		}
		
		lastMouseDown = e.getWhen();
		this.update();
	}
	
	public void mouseReleased(MouseEvent e) {
		if(e.getButton() == MouseEvent.BUTTON3) {
			if(mode == MODE_DRAWING) {
				selectedPoint = e.getPoint();
				selectVertex(e.getPoint(), DEFAULT_SELECTION_RADIUS);
				drawingPopupMenuDeleteVertex.setEnabled(selectedVertex != null);
				drawingPopupMenu.show(this, e.getX(), e.getY());
			}
			else if(mode == MODE_TILING) {
				selectedPoint = e.getPoint();
				selectSprite(e.getPoint());
				tilingPopupMenuBringSpriteToFront.setEnabled(selectedSprite != null);
				tilingPopupMenuSendSpriteToBack.setEnabled(selectedSprite != null);
				tilingPopupMenuDeleteSprite.setEnabled(selectedSprite != null);
				tilingPopupMenu.show(this, e.getX(), e.getY());
			}
		}
		else if(e.getButton() == MouseEvent.BUTTON2) {
			vertexToMove = null;
			spriteToMove = null;
		}
		else if(e.getButton() == MouseEvent.BUTTON1) {
			if(mode == MODE_DRAWING) {
				Vertex previousVertex = null;
				if(selectedVertex != null) {
					previousVertex = selectedVertex; 
				}
				selectedPoint = e.getPoint();
				selectVertex(e.getPoint(), DEFAULT_SELECTION_RADIUS);
				
				if(previousVertex != null && selectedVertex != null && !previousVertex.equals(selectedVertex)) {
					Edge newEdge = new Edge(previousVertex, selectedVertex);
					
					if(this.level.containsEdge(newEdge) ||
					   this.level.containsEdge(new Edge(selectedVertex, previousVertex))) {
						return;
					}
					
					int result = JOptionPane.showConfirmDialog(this, "Create edge?", "Edge Creation", JOptionPane.YES_NO_OPTION);
					if(result == JOptionPane.YES_OPTION) {
						this.level.addEdge(newEdge);
					}
				}
			}
			else if(mode == MODE_TILING) {
				if(selectedGridBlock != null && editorWindow.activeSprite != null) {
					Vertex v = new Vertex((editorWindow.activeSprite.isTiled()) ? selectedGridBlock.x : (int) (e.getX() - (editorWindow.activeSprite.getWidth() / 2.0f)),
							 			  (editorWindow.activeSprite.isTiled()) ? selectedGridBlock.y : (int) (e.getY() - (editorWindow.activeSprite.getHeight() / 2.0f)));
					Entity newEntity = new Entity(v, editorWindow.activeSprite);
					newEntity.spriteSheetIndex = editorWindow.spriteSheets.getSpriteSheetIndex(editorWindow.activeSprite.getParentName());
					if(newEntity.isTiled()) {
						level.addTile(newEntity);
					}
					else {
						level.addEntity(newEntity);
					}
				}
			}
		}
		this.update();
	}
	
	public void mouseDragged(MouseEvent e) {
		mousePosition = e.getPoint();
		if(mode == MODE_TILING) {
			getSelectedGridBlock(e.getPoint());
		}
		if(mode == MODE_DRAWING) {
			if(vertexToMove != null) {
				vertexToMove.x = e.getX();
				vertexToMove.y = e.getY();
			}
		}
		else if(mode == MODE_TILING) {
			if(spriteToMove != null) {
				spriteToMove.location.x = spriteToMove.isTiled() ? selectedGridBlock.x : (int) (e.getX() - (spriteToMove.getWidth() / 2.0f));
				spriteToMove.location.y = spriteToMove.isTiled() ? selectedGridBlock.y : (int) (e.getY() - (spriteToMove.getHeight() / 2.0f));
			}
		}
		this.update();
	}
	
	public void mouseMoved(MouseEvent e) {
		mousePosition = e.getPoint();
		if(mode == MODE_TILING) {
			getSelectedGridBlock(e.getPoint());
		}
		this.update();
	}
	
	public void actionPerformed(ActionEvent e) {
		if(level == null) { return; }
		
		if(mode == MODE_DRAWING) {
			if(e.getSource() == drawingPopupMenuNewVertex) {
				Vertex newVertex = new Vertex(selectedPoint.x, selectedPoint.y);
				level.addVertex(newVertex);
				lastSelectedVertex = null;
				selectedVertex = null;
			}
			else if(e.getSource() == drawingPopupMenuDeleteVertex) {
				level.removeVertex(selectedVertex);
				selectedVertex = null;
			}
			else if(e.getSource() == drawingPopupMenuTileMode) {
				mode = MODE_TILING;
			}
		}
		else if(mode == MODE_TILING) {
			if(e.getSource() == tilingPopupMenuBringSpriteToFront) {
				level.bringSpriteToFront(selectedSprite);
			}
			else if(e.getSource() == tilingPopupMenuSendSpriteToBack) {
				level.sendSpriteToBack(selectedSprite);
			}
			else if(e.getSource() == tilingPopupMenuDeleteSprite) {
				level.deleteSprite(selectedSprite);
			}
			else if(e.getSource() == tilingPopupMenuDrawMode) {
				mode = MODE_DRAWING;
			}
		}
		this.update();
	}
	
	public void selectVertex(Point p, int r) {
		if(p == null) { return; }
		if(r < 0) { r = 6; }
		selectedVertex = null;
		if(level != null) {
			for(int i=0;i<level.numberOfVertices();i++) {
				Vertex v = level.getVertex(i);
				if(Math.sqrt( Math.pow( (selectedPoint.x - v.x) , 2) + Math.pow( (selectedPoint.y - v.y) , 2) ) <= r) {
					selectedVertex = v;
					lastSelectedVertex = selectedVertex;
				}
			}
		}
	}
	
	public void selectSprite(Point p) {
		if(p == null) { return; }
		selectedSprite = null;
		if(level != null) {
			if(selectedSprite == null) {
				for(int i=level.numberOfEntities()-1;i>=0;i--) {
					Entity e = level.getEntity(i);
					if(p.x >= e.location.x &&
					   p.y >= e.location.y &&
					   p.x <= e.location.x + e.getWidth() &&
					   p.y <= e.location.y + e.getHeight()) {
						selectedSprite = e;
						break;
					}
				}
			}
			if(selectedSprite == null) {
				for(int i=level.numberOfTiles()-1;i>=0;i--) {
					Entity e = level.getTile(i);
					if(selectedGridBlock.x >= e.location.x &&
					   selectedGridBlock.y >= e.location.y &&
					   selectedGridBlock.x <= e.location.x + (e.getWidth() / Level.GRID_SIZE) - 1 &&
					   selectedGridBlock.y <= e.location.y + (e.getHeight() / Level.GRID_SIZE) - 1) {
						selectedSprite = e;
						break;
					}
				}
			}
		}
	}
	
	public void getSelectedGridBlock(Point p) {
		if(level == null) { return; }
		
		Point current = p;
		Point offset = new Point(current.x, current.y);
		Point location = new Point(offset.x / Level.GRID_SIZE,
				 				   offset.y / Level.GRID_SIZE);
		if(location.x < 0 || location.y < 0 || location.x >= level.gridSize.x || location.y >= level.gridSize.y) {
			selectedGridBlock = null;
		}
		selectedGridBlock = location;
	}
	
	public void paintComponent(Graphics g) {
		super.paintComponent(g);
		
		g.clearRect(0, 0, this.getWidth(), this.getHeight());
		
		g.setColor(backgroundColour);
		g.fillRect(0, 0, this.getWidth(), this.getHeight());
		
		drawObjects(g);
		
		drawGrid(g);
		
		if(mode == MODE_TILING && selectedGridBlock != null && editorWindow.activeSprite != null) {
			int xPos = editorWindow.activeSprite.isTiled() ? selectedGridBlock.x * Level.GRID_SIZE : (mousePosition == null) ? 0 : (int) (mousePosition.x - (editorWindow.activeSprite.getWidth() / 2.0f));
			int yPos = editorWindow.activeSprite.isTiled() ? selectedGridBlock.y * Level.GRID_SIZE : (mousePosition == null) ? 0 : (int) (mousePosition.y - (editorWindow.activeSprite.getHeight() / 2.0f));
			editorWindow.activeSprite.paintOn(g, xPos, yPos);
		}
		
		if(collisionLinesEnabled && level != null) {
			level.paintOn(g, lineColour, vertexColour);
		}
		
		g.setColor(selectedColour);
		if(mode == MODE_DRAWING) {
			if(vertexToMove != null) {
				vertexToMove.paintOn(g);
			}
			else if(selectedVertex != null) {
				selectedVertex.paintOn(g);
			}
		}
	}
	
	public void drawGrid(Graphics g) {
		if(level != null) {
			g.setColor(gridColour);
			
			if(gridEnabled) {
				for(int i=0;i<level.gridSize.x+1;i++) {
					g.drawLine(i * Level.GRID_SIZE, 0, i * Level.GRID_SIZE, level.gridSize.y * Level.GRID_SIZE);
				}
				
				for(int j=0;j<level.gridSize.y+1;j++) {
					g.drawLine(0, j * Level.GRID_SIZE, level.gridSize.x * Level.GRID_SIZE, j * Level.GRID_SIZE);
				}
				
				if(mode == MODE_TILING && selectedGridBlock != null) {
					Sprite sprite = null;
					int x = 0, y = 0;
					if(editorWindow.activeSprite != null && editorWindow.activeSprite.isTiled()) {
						sprite = editorWindow.activeSprite;
						x = selectedGridBlock.x;
						y = selectedGridBlock.y;
					}
					else if(spriteToMove != null && spriteToMove.isTiled()) {
						sprite = spriteToMove.getSprite();
						x = spriteToMove.location.x;
						y = spriteToMove.location.y;
					}
					if(sprite != null) {
						Graphics2D g2 = (Graphics2D) g;
						Stroke s = g2.getStroke();
						g2.setStroke(new BasicStroke(2));
						g2.setColor(selectedColour);
						int d = Level.GRID_SIZE;
						int w = sprite.getWidth();
						int h = sprite.getHeight();
						g2.drawLine( x*d,     y*d,   (x*d)+w,  y*d);
						g2.drawLine((x*d)+w,  y*d,   (x*d)+w, (y*d)+h);
						g2.drawLine((x*d)+w, (y*d)+h, x*d,    (y*d)+h);
						g2.drawLine( x*d,    (y*d)+h, x*d,     y*d);
						g2.setStroke(s);
					}
				}
			}
			else {
				g.drawLine(0, 0, 0, level.gridSize.y * Level.GRID_SIZE);
				g.drawLine(level.gridSize.x * Level.GRID_SIZE, 0, level.gridSize.x * Level.GRID_SIZE, level.gridSize.y * Level.GRID_SIZE);
				g.drawLine(0, 0, level.gridSize.x * Level.GRID_SIZE, 0);
				g.drawLine(0, level.gridSize.y * Level.GRID_SIZE, level.gridSize.x * Level.GRID_SIZE, level.gridSize.y * Level.GRID_SIZE);
			}
		}
	}
	
	public void drawObjects(Graphics g) {
		if(level == null) { return; }
		
		for(int i=0;i<level.numberOfTiles();i++) {
			level.getTile(i).paintOn(g);
		}
		
		for(int i=0;i<level.numberOfEntities();i++) {
			level.getEntity(i).paintOn(g);
		}
	}
	
	public void reset() {
		selectedVertex = null;
		vertexToMove = null;
		lastSelectedVertex = null;
		
		selectedSprite = null;
		spriteToMove = null;
	}
	
	public void update() {
		this.repaint();
	}
}
