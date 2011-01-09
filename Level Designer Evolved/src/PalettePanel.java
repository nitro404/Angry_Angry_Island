// ======================================= //
// Level Designer Evolved                  //
//                                         //
// Author: Kevin Scroggins                 //
// E-Mail: nitro404@hotmail.com            //
// ======================================= //

import java.awt.*;
import java.awt.event.*;
import javax.swing.*;
import java.util.Vector;

public class PalettePanel extends JPanel implements Scrollable, ActionListener {
	
	private static final long serialVersionUID = 1L;
	
	private EditorWindow editorWindow;
	
	private Vector<JButton> spriteButtons;
	private Vector<Point> spriteLocations;
	
	private Dimension panelSize; 
	
	public PalettePanel(EditorWindow editorWindow) {
		this.editorWindow = editorWindow;
		if(editorWindow == null) {
			System.out.println("ERROR: Editor Window cannot be null.");
			System.exit(1);
		}
		
		this.setLayout(null);
		
		createPalette();
		
		update();
	}
	
	private void createPalette() {
		spriteButtons = new Vector<JButton>(30);
		spriteLocations = new Vector<Point>(30);
		int yPos = 5;
		int maxWidth = 5;
		for(int i=0;i<editorWindow.spriteSheets.size();i++) {
			for(int j=0;j<editorWindow.spriteSheets.elementAt(i).size();j++) {
				Sprite r = editorWindow.spriteSheets.elementAt(i).elementAt(j);
				
				if(r.getName() != null &&
				   r.getType() == Sprite.TYPE_TILE ||
				   r.getType() == Sprite.TYPE_OBJECT) {
					
					Image s = r.getImage();
					ImageIcon c = new ImageIcon();
					c.setImage(s);
					JButton b = new JButton(c);
					b.addActionListener(this);
					b.setBackground(new Color(255, 255, 255));
					b.setSize(new Dimension(r.getDimensions()));
					b.setBorder(null);
					b.setBorderPainted(false);
					this.add(b);
					b.setLocation(5, yPos);
					yPos += r.getHeight() + 5;
					spriteButtons.add(b);
					spriteLocations.add(new Point(i, j));
					if(r.getWidth() + 20 > maxWidth) { maxWidth = r.getWidth() + 20; }
				}
			}
		}
		panelSize = new Dimension(maxWidth, yPos);
		int xPos = this.getSize().width;
		this.setSize(new Dimension(xPos, yPos));
	}
	
	public Dimension getPreferredSize() {
		if(panelSize != null) {
			return panelSize;
		}
		else {
			return new Dimension(160, 728);
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
			return visibleRect.width - 5;
		}
		else {
			return visibleRect.height - 5;
		}
	}
	
	public boolean getScrollableTracksViewportHeight() {
		return false;
	}

	public boolean getScrollableTracksViewportWidth() {
		return false;
	}
	
	public void actionPerformed(ActionEvent e) {
		for(int i=0;i<spriteButtons.size();i++) {
			if(e.getSource() == spriteButtons.elementAt(i)) {
				editorWindow.activeSprite = editorWindow.spriteSheets.elementAt(spriteLocations.elementAt(i).x).elementAt(spriteLocations.elementAt(i).y);
				break;
			}
		}
		this.update();
		editorWindow.update();
	}
		
	public void paintComponent(Graphics g) {
		super.paintComponent(g);
		
		g.clearRect(0, 0, this.getWidth(), this.getHeight());
	}
	
	public void update() {
		this.repaint();
	}
	
}
