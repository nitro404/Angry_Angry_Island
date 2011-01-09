// ======================================= //
// Level Designer Evolved                  //
//                                         //
// Author: Kevin Scroggins                 //
// E-Mail: nitro404@hotmail.com            //
// ======================================= //

import java.io.*;
import java.util.StringTokenizer;
import java.awt.Dimension;
import java.awt.Point;
import java.awt.event.*;
import javax.swing.*;

public class EditorWindow extends JFrame implements ActionListener {
	
	private static final long serialVersionUID = 1L;
	
	final public static String DEFAULT_SETTINGS_FILE = "settings.ini";
	final public static String DEFAULT_SPRITESHEET_FILE = "spritesheets.ini";
	final public static String DEFAULT_MAP_DIRECTORY = "../Maps";
	final public static String DEFAULT_SPRITE_DIRECTORY = "../Sprites";
	final public static String ALTERNATE_SETTINGS_FILE = "../settings.ini";
	final public static String ALTERNATE_SPRITESHEET_FILE = "../spritesheets.ini";
	final public static String ALTERNATE_MAP_DIRECTORY = "../../Maps";
	final public static String ALTERNATE_SPRITE_DIRECTORY = "../../Sprites";
	private int DEFAULT_EDITOR_WIDTH = 1080;
	private int DEFAULT_EDITOR_HEIGHT = 768;
	private int DEFAULT_XPOS = 0;
	private int DEFAULT_YPOS = 0;
	
	private String settingsFileName;
	private String spriteSheetFileName;
	public VariableSystem settings;
	public String mapDirectory;
	public String spriteDirectory;
	private Dimension initialEditorSize;
	private Point initialEditorPosition;
	
	public Sprite activeSprite;
	public SpriteSheets spriteSheets; 
	
	public Level level;
	
	final public static int DEFAULT_WIDTH = 18;
	final public static int DEFAULT_HEIGHT = 12;
	
	private JMenuBar menu;
	private JMenu menuFile;
	private JMenuItem menuFileNewMap;
	private JMenuItem menuFileOpenMap;
	private JMenuItem menuFileSaveMap;
	private JMenuItem menuFileSaveSettings;
	private JMenuItem menuFileResetSettings;
	private JMenuItem menuFileExit;
	private JMenu menuView;
	private JMenuItem menuViewPalette;
	private JMenuItem menuViewDimensions;
	private JMenuItem menuViewCollisionLinesToggle;
	private JMenuItem menuViewGridToggle;
	private JMenuItem menuViewGridColour;
	private JMenuItem menuViewLineColour;
	private JMenuItem menuViewVertexColour;
	private JMenuItem menuViewSelectedColour;
	private JMenuItem menuViewBackgroundColour;
	private JMenu menuMode;
	private JMenuItem menuModeTile;
	private JMenuItem menuModeDraw;
	private JMenu menuHelp;
	private JMenuItem menuHelpAbout;
	
	public EditorPanel editorPanel;
	private JScrollPane editorPanelScrollPane;
	
	private PaletteWindow paletteWindow;
	
	private JFileChooser fileChooser; 
	
	public EditorWindow(String settingsFileName) {
		super("Level Designer Evolved");
		setDefaultCloseOperation(EXIT_ON_CLOSE);
		
		loadSettings(settingsFileName);
		
		loadImages();
		
		setSize(initialEditorSize.width, initialEditorSize.height);
		setLocation(initialEditorPosition.x, initialEditorPosition.y);
		
		fileChooser = new JFileChooser();
		fileChooser.setCurrentDirectory(new File(System.getProperty("user.dir") + "\\" + mapDirectory).getAbsoluteFile());
		
		createMenu();
		
		editorPanel = new EditorPanel(this, settings);
		editorPanelScrollPane = new JScrollPane(editorPanel);
		add(editorPanelScrollPane);
		
		paletteWindow = new PaletteWindow(this, settings);
		paletteWindow.setVisible(true);
		
		createNewMap();
		
		update();
	}
	
	public void loadImages() {
		try {
			if(spriteSheetFileName == null || spriteDirectory == null) {
				System.out.println("ERROR: Sprite directory or sprite sheet file not specified.");
				System.exit(1);
			}
			spriteSheets = SpriteSheets.parseFrom(spriteSheetFileName, spriteDirectory);
			if(spriteSheets == null) {
				System.out.println("ERROR: Unable to parse sprite sheet(s) from specified data file.");
				System.exit(1);
			}
			activeSprite = null;
		}
		catch(Exception e) { }
	}
	
	private boolean loadSettings(String fileName) {
		this.settingsFileName = Utilities.getValidFile(fileName, DEFAULT_SETTINGS_FILE, ALTERNATE_SETTINGS_FILE);
		
		settings = new VariableSystem();
		boolean settingsLoaded = settings.parseFrom(this.settingsFileName);
		
		this.spriteSheetFileName = Utilities.getValidFile(settings.getValue("SpriteSheet File"), DEFAULT_SPRITESHEET_FILE, ALTERNATE_SPRITESHEET_FILE);
		this.mapDirectory = Utilities.getValidDirectory(settings.getValue("Map Directory"), DEFAULT_MAP_DIRECTORY, ALTERNATE_MAP_DIRECTORY);
		this.spriteDirectory = Utilities.getValidDirectory(settings.getValue("Sprite Directory"), DEFAULT_SPRITE_DIRECTORY, ALTERNATE_SPRITE_DIRECTORY);
		
		initialEditorPosition = Utilities.parsePoint(settings.getValue("Editor Window Position"));
		if(initialEditorPosition == null) { initialEditorPosition = new Point(DEFAULT_XPOS, DEFAULT_YPOS); }
		
		initialEditorSize = Utilities.parseDimension(settings.getValue("Editor Window Size"));
		if(initialEditorSize == null) { initialEditorSize = new Dimension(DEFAULT_EDITOR_WIDTH, DEFAULT_EDITOR_HEIGHT); }
		
		if(!settingsLoaded) {
			settings = null;
			return false;
		}
		
		return true;
	}
	
	private void saveSettings() {
		PrintWriter out;
		try {
			out = new PrintWriter(new FileWriter(this.settingsFileName));
			out.println("Map Directory=" + this.mapDirectory);
			out.println("Sprite Directory=" + this.spriteDirectory);
			out.println("SpriteSheet File=" + this.spriteSheetFileName);
			out.println("Editor Window Position=" + this.getLocation().x + ", " + this.getLocation().y);
			out.println("Editor Window Size=" + this.getWidth() + ", " + this.getHeight());
			out.println("Palette Window Position=" + this.paletteWindow.getLocation().x + ", " + this.paletteWindow.getLocation().y);
			out.println("Palette Window Size=" + this.paletteWindow.getWidth() + ", " + this.paletteWindow.getHeight());
			out.println("Grid Colour=" + this.editorPanel.gridColour.getRed() + ", " + this.editorPanel.gridColour.getGreen() + ", " + this.editorPanel.gridColour.getBlue());
			out.println("Line Colour=" + this.editorPanel.lineColour.getRed() + ", " + this.editorPanel.lineColour.getGreen() + ", " + this.editorPanel.lineColour.getBlue());
			out.println("Vertex Colour=" + this.editorPanel.vertexColour.getRed() + ", " + this.editorPanel.vertexColour.getGreen() + ", " + this.editorPanel.vertexColour.getBlue());
			out.println("Selected Colour=" + this.editorPanel.selectedColour.getRed() + ", " + this.editorPanel.selectedColour.getGreen() + ", " + this.editorPanel.selectedColour.getBlue());
			out.close();
		}
		catch(IOException e) { }
	}
	
	private void resetSettings() {
		this.mapDirectory = DEFAULT_MAP_DIRECTORY;
		this.spriteDirectory = DEFAULT_SPRITE_DIRECTORY;
		this.spriteSheetFileName = DEFAULT_SPRITESHEET_FILE;
		this.setLocation(DEFAULT_XPOS, DEFAULT_YPOS);
		this.setSize(DEFAULT_EDITOR_WIDTH, DEFAULT_EDITOR_HEIGHT);
		this.paletteWindow.setLocation(this.getX() + this.getWidth(), this.getY());
		this.paletteWindow.setSize(PaletteWindow.DEFAULT_PALETTE_WIDTH, PaletteWindow.DEFAULT_PALETTE_HEIGHT);
		this.editorPanel.gridColour = EditorPanel.DEFAULT_GRID_COLOUR;
		this.editorPanel.lineColour = EditorPanel.DEFAULT_LINE_COLOUR;
		this.editorPanel.vertexColour = EditorPanel.DEFAULT_VERTEX_COLOUR;
		this.editorPanel.selectedColour = EditorPanel.DEFAULT_SELECTED_COLOUR;
		this.editorPanel.backgroundColour = EditorPanel.DEFAULT_BACKGROUND_COLOUR;
	}
	
	private void createMenu() {
		menu = new JMenuBar();
		
		menuFile = new JMenu("File");
		menuFileNewMap = new JMenuItem("New Map");
		menuFileOpenMap = new JMenuItem("Open Map");
		menuFileSaveMap = new JMenuItem("Save Map");
		menuFileSaveSettings = new JMenuItem("Save Settings");
		menuFileResetSettings = new JMenuItem("Reset Settings");
		menuFileExit = new JMenuItem("Exit");
		menuView = new JMenu("View");
		menuViewPalette = new JMenuItem("Palette");
		menuViewDimensions = new JMenuItem("Map Dimensions");
		menuViewCollisionLinesToggle = new JMenuItem("Toggle Collision Lines");
		menuViewGridToggle = new JMenuItem("Toggle Grid");
		menuViewGridColour = new JMenuItem("Grid Colour");
		menuViewLineColour = new JMenuItem("Line Colour");
		menuViewVertexColour = new JMenuItem("Vertex Colour");
		menuViewSelectedColour = new JMenuItem("Selected Colour");
		menuViewBackgroundColour = new JMenuItem("Background Colour");
		menuMode = new JMenu("Mode");
		menuModeTile = new JMenuItem("Tile Textures");
		menuModeDraw = new JMenuItem("Draw Boundaries");
		menuHelp = new JMenu("Help");
		menuHelpAbout = new JMenuItem("About");
			
		menuFileNewMap.addActionListener(this);
		menuFileOpenMap.addActionListener(this);
		menuFileSaveMap.addActionListener(this);
		menuFileSaveSettings.addActionListener(this);
		menuFileResetSettings.addActionListener(this);
		menuFileExit.addActionListener(this);
		menuViewPalette.addActionListener(this);
		menuViewDimensions.addActionListener(this);
		menuViewCollisionLinesToggle.addActionListener(this);
		menuViewGridToggle.addActionListener(this);
		menuViewGridColour.addActionListener(this);
		menuViewLineColour.addActionListener(this);
		menuViewVertexColour.addActionListener(this);
		menuViewSelectedColour.addActionListener(this);
		menuViewBackgroundColour.addActionListener(this);
		menuMode.addActionListener(this);
		menuModeTile.addActionListener(this);
		menuModeDraw.addActionListener(this);
		menuHelpAbout.addActionListener(this);
		
		menuFile.add(menuFileNewMap);
		menuFile.add(menuFileOpenMap);
		menuFile.add(menuFileSaveMap);
		menuFile.addSeparator();
		menuFile.add(menuFileSaveSettings);
		menuFile.add(menuFileResetSettings);
		menuFile.add(menuFileExit);
		menuView.add(menuViewPalette);
		menuView.add(menuViewDimensions);
		menuView.add(menuViewCollisionLinesToggle);
		menuView.add(menuViewGridToggle);
		menuView.addSeparator();
		menuView.add(menuViewGridColour);
		menuView.add(menuViewLineColour);
		menuView.add(menuViewVertexColour);
		menuView.add(menuViewSelectedColour);
		menuView.add(menuViewBackgroundColour);
		menuMode.add(menuModeTile);
		menuMode.add(menuModeDraw);
		menuHelp.add(menuHelpAbout);
		
		menu.add(menuFile);
		menu.add(menuView);
		menu.add(menuMode);
		menu.add(menuHelp);
		
		setJMenuBar(menu);
	}
	
	public void actionPerformed(ActionEvent e) {
		if(e.getSource().equals(menuFileNewMap)) {
			createNewMap();
		}
		else if(e.getSource() == menuFileOpenMap) {
			if(fileChooser.showDialog(this, "Open Map") == JFileChooser.APPROVE_OPTION) {
				level = Level.parseFrom(fileChooser.getSelectedFile().getAbsolutePath(), spriteSheets);
				editorPanel.setLevel(level);
			}
		}
		else if(e.getSource() == menuFileSaveMap) {
			if(fileChooser.showDialog(this, "Save Map") == JFileChooser.APPROVE_OPTION) {
				String fileName = fileChooser.getSelectedFile().getAbsolutePath();
				if(!Utilities.hasExtension(fileName, "2d")) {
					fileName += ".2d";
				}
				level.writeTo(fileName);
			}
		}
		else if(e.getSource() == menuFileSaveSettings) {
			saveSettings();
		}
		else if(e.getSource() == menuFileResetSettings) {
			resetSettings();
		}
		else if(e.getSource() == menuFileExit) {
			System.exit(0);
		}
		else if(e.getSource() == menuViewPalette) {
			paletteWindow.setVisible(!paletteWindow.isVisible());
		}
		else if(e.getSource() == menuViewDimensions) {
			setMapDimensions(level);
		}
		else if(e.getSource() == menuViewCollisionLinesToggle) {
			editorPanel.collisionLinesEnabled = !editorPanel.collisionLinesEnabled;
		}
		else if(e.getSource() == menuViewGridToggle) {
			editorPanel.gridEnabled = !editorPanel.gridEnabled;
		}
		else if(e.getSource() == menuViewGridColour) {
			editorPanel.gridColour = JColorChooser.showDialog(this, "Choose Grid Colour", editorPanel.gridColour);
		}
		else if(e.getSource() == menuViewLineColour) {
			editorPanel.lineColour = JColorChooser.showDialog(this, "Choose Line Colour", editorPanel.lineColour);
		}
		else if(e.getSource() == menuViewVertexColour) {
			editorPanel.vertexColour = JColorChooser.showDialog(this, "Choose Vertex Colour", editorPanel.vertexColour);
		}
		else if(e.getSource() == menuViewSelectedColour) {
			editorPanel.selectedColour = JColorChooser.showDialog(this, "Choose Selected Colour", editorPanel.selectedColour);
		}
		else if(e.getSource() == menuViewBackgroundColour) {
			editorPanel.backgroundColour = JColorChooser.showDialog(this, "Choose Background Colour", editorPanel.backgroundColour);
		}
		else if(e.getSource() == menuModeTile) {
			editorPanel.mode = EditorPanel.MODE_TILING;
		}
		else if(e.getSource() == menuModeDraw) {
			editorPanel.mode = EditorPanel.MODE_DRAWING;
		}
		else if(e.getSource() == menuHelpAbout) {
			JOptionPane.showMessageDialog(this, "Level Designer Evolved created by Kevin Scroggins.", "Level Designer Evolved", JOptionPane.INFORMATION_MESSAGE);
		}
		
		this.update();
	}
	
	public void createNewMap() {
		this.level = new Level();
		editorPanel.setLevel(level);
		editorPanel.reset();
		setMapDimensions(level, DEFAULT_WIDTH, DEFAULT_HEIGHT);
	}
	
	public void setMapDimensions(Level level) {
		String data = JOptionPane.showInputDialog(this, "Please enter new map dimensions (ie. " + DEFAULT_WIDTH + "x" + DEFAULT_HEIGHT + ").", "Map Dimensions", JOptionPane.QUESTION_MESSAGE);
		if(data != null) {
			StringTokenizer st = new StringTokenizer(data.trim(), ",x");
			int newWidth = 0, newHeight = 0;
			boolean valid = true;
			if(st.countTokens() != 2) { valid = false; }
			if(valid) {
				try {
					newWidth = Integer.parseInt(st.nextToken().trim());
					newHeight = Integer.parseInt(st.nextToken().trim());
				}
				catch(NumberFormatException e) {
					valid = false;
				}
				if(newWidth <= 0 || newHeight <= 0) {
					valid = false;
				}
			}
			
			if(valid) {
				setMapDimensions(level, newWidth, newHeight);
			}
			else {
				JOptionPane.showMessageDialog(this, "Invalid map dimension entered, please the form \"AxB\" or \"A, B\".", "Invalid Dimension", JOptionPane.ERROR_MESSAGE);
			}
		}
		update();
	}
	
	public void setMapDimensions(Level level, int newWidth, int newHeight) {
		level.gridSize = new Vertex(newWidth, newHeight);
		level.dimensions = new Dimension((newWidth * Level.GRID_SIZE) + 1, (newHeight * Level.GRID_SIZE) + 1);
	}
	
	public void update() {
		editorPanelScrollPane.revalidate();
		editorPanelScrollPane.repaint();
		editorPanel.update();
		paletteWindow.update();
		this.repaint();
	}
	
}
