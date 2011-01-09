// ======================================= //
// Level Designer Evolved                  //
//                                         //
// Author: Kevin Scroggins                 //
// E-Mail: nitro404@hotmail.com            //
// ======================================= //

import java.awt.Dimension;
import java.awt.Point;
import java.awt.event.*;

import javax.swing.*;

public class PaletteWindow extends JFrame implements ActionListener {
	
	private static final long serialVersionUID = 1L;
	
	final public static int DEFAULT_PALETTE_WIDTH = 200;
	final public static int DEFAULT_PALETTE_HEIGHT = 768;
	
	private Point initialPalettePosition;
	private Dimension initialPaletteSize;

	private PalettePanel palettePanel;
	private JScrollPane palettePanelScrollPane;
	
	public PaletteWindow(EditorWindow editorWindow, VariableSystem settings) {
		super("Palette");
		
		loadSettings(settings, editorWindow);
		
		setSize(initialPaletteSize.width, initialPaletteSize.height);
		setLocation(initialPalettePosition.x, initialPalettePosition.y);
		
		palettePanel = new PalettePanel(editorWindow);
		palettePanelScrollPane = new JScrollPane(palettePanel);
		add(palettePanelScrollPane);
		
		update();
	}
	
	private void loadSettings(VariableSystem settings, EditorWindow editorWindow) {
		if(editorWindow == null) { initialPalettePosition = new Point(0, 0); }
		else { initialPalettePosition = new Point(editorWindow.getX() + editorWindow.getWidth(), editorWindow.getY()); }
		initialPaletteSize = new Dimension(DEFAULT_PALETTE_WIDTH, DEFAULT_PALETTE_HEIGHT);
		
		if(settings != null) {
			Point p;
			Dimension d;
			if((p = Utilities.parsePoint(settings.getValue("Palette Window Position"))) != null) { initialPalettePosition = p; }
			if((d = Utilities.parseDimension(settings.getValue("Palette Window Size"))) != null) { initialPaletteSize = d; }
		}
	}
	
	public void actionPerformed(ActionEvent e) { update(); }
	

	public void update() {
		palettePanel.update();
		palettePanelScrollPane.repaint();
		palettePanelScrollPane.revalidate();
		this.repaint();
	}
	
}
