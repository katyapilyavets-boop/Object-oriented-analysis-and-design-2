package com.example;

import javax.swing.*;
import java.awt.*;
import java.sql.*;
import java.util.ArrayList;
import java.util.List;


// МОДЕЛИ ДАННЫХ


class Reader {
    private int id;
    private String name;
    private String phone;

    public Reader(int id, String name, String phone) {
        this.id = id;
        this.name = name;
        this.phone = phone;
    }

    public int getId() { return id; }
    public String getName() { return name; }
    public String getPhone() { return phone; }

    @Override
    public String toString() { return name; }
}

class Book {
    private int id;
    private String title;
    private String author;

    public Book(int id, String title, String author) {
        this.id = id;
        this.title = title;
        this.author = author;
    }

    public int getId() { return id; }
    public String getTitle() { return title; }
    public String getAuthor() { return author; }

    @Override
    public String toString() { 
        return title + " (" + author + ")"; 
    }
}

// РЕПОЗИТОРИЙ — ПРЯМЫЕ ЗАПРОСЫ К БД 


class LibrarySession {
    private static final String DB_URL = "jdbc:mysql://localhost:3306/ooap_test?useSSL=false&allowPublicKeyRetrieval=true";
    private static final String DB_USER = "root";
    private static final String DB_PASS = "katikkotik"; 

    private int queryCount = 0;

    public Connection getConnection() throws SQLException {
        return DriverManager.getConnection(DB_URL, DB_USER, DB_PASS);
    }

    public List<Book> getAllBooksForList() {
        List<Book> list = new ArrayList<>();
        String sql = "SELECT id, title, author FROM books";
        try (Connection conn = getConnection(); 
             Statement stmt = conn.createStatement();
             ResultSet rs = stmt.executeQuery(sql)) {
            while (rs.next()) {
                list.add(new Book(rs.getInt("id"), rs.getString("title"), rs.getString("author")));
            }
        } catch (SQLException e) { e.printStackTrace(); }
        return list;
    }

    
    public Book getBookDetails(int id) {
        queryCount++;
        String sql = "SELECT * FROM books WHERE id = ?";
        try (Connection conn = getConnection(); 
             PreparedStatement ps = conn.prepareStatement(sql)) {
            ps.setInt(1, id);
            ResultSet rs = ps.executeQuery();
            if (rs.next()) {
                return new Book(rs.getInt("id"), rs.getString("title"), rs.getString("author"));
            }
        } catch (SQLException e) { e.printStackTrace(); }
        return null;
    }


    public Reader getReader(int id) {
        queryCount++;
        String sql = "SELECT * FROM readers WHERE id = ?";
        try (Connection conn = getConnection(); 
             PreparedStatement ps = conn.prepareStatement(sql)) {
            ps.setInt(1, id);
            ResultSet rs = ps.executeQuery();
            if (rs.next()) {
                return new Reader(rs.getInt("id"), rs.getString("full_name"), rs.getString("phone"));
            }
        } catch (SQLException e) { e.printStackTrace(); }
        return null;
    }

    // 🔽 Поиск держателя
    public Reader findHolder(int bookId) {
        queryCount++;
        String sql = "SELECT reader_id FROM loans WHERE book_id = ? AND return_date IS NULL";
        try (Connection conn = getConnection(); 
             PreparedStatement ps = conn.prepareStatement(sql)) {
            ps.setInt(1, bookId);
            ResultSet rs = ps.executeQuery();
            if (rs.next()) {
                int readerId = rs.getInt("reader_id");
                return getReader(readerId);
            }
        } catch (SQLException e) { e.printStackTrace(); }
        return null;
    }

    public void resetCounters() { queryCount = 0; }
    public int getQueryCount() { return queryCount; }

    // Метод log оставлен пустым — логируем только в GUI
    private void log(String msg) {
        if (Main.getInstance() != null) Main.getInstance().addLog(msg);
    }
}


// GUI — ЧИСТЫЕ ЛОГИ


public class Main extends JFrame {
    private static Main instance;
    private final LibrarySession session;
    
    private JComboBox<Book> comboBook;
    private JTextArea logArea, resultArea;
    private JLabel lblCounters;

    public Main() {
        instance = this;
        session = new LibrarySession();
        initUI();
        loadBooks();
    }

    public static Main getInstance() { return instance; }

    private void initUI() {
        setTitle("📚 Библиотека");
        setSize(900, 650);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setLocationRelativeTo(null);

        // 🔹 Верхняя панель
        JPanel topPanel = new JPanel();
        topPanel.setLayout(new BoxLayout(topPanel, BoxLayout.Y_AXIS));
        topPanel.setBorder(BorderFactory.createEmptyBorder(15, 15, 15, 15));
        
        JPanel selectPanel = new JPanel(new FlowLayout(FlowLayout.LEFT, 10, 5));
        selectPanel.add(new JLabel("📚 Выберите книгу:"));
        comboBook = new JComboBox<>();
        comboBook.setPreferredSize(new Dimension(450, 28));
        selectPanel.add(comboBook);
        
        JPanel buttonPanel = new JPanel(new FlowLayout(FlowLayout.LEFT, 10, 5));
        
        JButton btnCheck = new JButton("🔍 Проверить");
        btnCheck.setFont(new Font("Segoe UI", Font.BOLD, 12));
        btnCheck.setPreferredSize(new Dimension(150, 35));
        btnCheck.setMaximumSize(new Dimension(150, 35));
        btnCheck.addActionListener(e -> checkBook());
        
        buttonPanel.add(btnCheck);
        
        topPanel.add(selectPanel);
        topPanel.add(Box.createVerticalStrut(10));
        topPanel.add(buttonPanel);

        // 🔹 Центральная часть
        JPanel centerPanel = new JPanel(new BorderLayout(10, 10));
        centerPanel.setBorder(BorderFactory.createEmptyBorder(0, 15, 10, 15));

        logArea = new JTextArea(12, 0);
        logArea.setEditable(false);
        logArea.setFont(new Font("Consolas", Font.PLAIN, 11));
        logArea.setBackground(new Color(255, 250, 250));
        JScrollPane scrollLog = new JScrollPane(logArea);
        scrollLog.setBorder(BorderFactory.createTitledBorder("📋 Лог операций"));

        resultArea = new JTextArea(6, 0);
        resultArea.setEditable(false);
        resultArea.setFont(new Font("Segoe UI", Font.PLAIN, 13));
        resultArea.setBackground(new Color(255, 255, 255));
        resultArea.setLineWrap(true);
        resultArea.setWrapStyleWord(true);
        JScrollPane scrollResult = new JScrollPane(resultArea);
        scrollResult.setBorder(BorderFactory.createTitledBorder("📊 Результат"));

        centerPanel.add(scrollLog, BorderLayout.CENTER);
        centerPanel.add(scrollResult, BorderLayout.SOUTH);

        // 🔹 Нижняя панель
        JPanel bottomPanel = new JPanel(new FlowLayout(FlowLayout.LEFT, 15, 10));
        lblCounters = new JLabel("Запросов к БД: 0");
        lblCounters.setFont(new Font("Segoe UI", Font.BOLD, 12));
        bottomPanel.add(lblCounters);

        add(topPanel, BorderLayout.NORTH);
        add(centerPanel, BorderLayout.CENTER);
        add(bottomPanel, BorderLayout.SOUTH);
    }

    private void loadBooks() {
        addLog("Загрузка списка книг...");
        for (Book b : session.getAllBooksForList()) {
            comboBook.addItem(b);
        }
        addLog("✅ Готово. Выберите книгу для проверки.\n");
    }

    private void checkBook() {
        Book selectedBook = (Book) comboBook.getSelectedItem();
        if (selectedBook == null) {
            JOptionPane.showMessageDialog(this, "Выберите книгу");
            return;
        }

        session.resetCounters();
        resultArea.setText("");
        
        addLog("Проверка: " + selectedBook.getTitle());

        // 1. Загружаем данные книги
        Book book = session.getBookDetails(selectedBook.getId());
        if (book == null) {
            resultArea.setText("❌ Книга не найдена");
            return;
        }

        // 2. Проверяем, кто держит книгу
        Reader holder = session.findHolder(book.getId());

        if (holder == null) {
            resultArea.setForeground(Color.GREEN.darker());
            resultArea.setText("✅ Книга доступна\n\n" + book.getTitle());
            addLog("→ Статус: свободна");
        } else {
            resultArea.setForeground(Color.RED.darker());
            resultArea.setText("🔴 Книга занята\n\n" + 
                             "Читатель: " + holder.getName() + "\n" +
                             "Телефон: " + holder.getPhone());
            addLog("→ Статус: занята (" + holder.getName() + ")");
        }

        // Итог
        addLog("Запросов к БД: " + session.getQueryCount() + "\n");
        lblCounters.setText("Запросов к БД: " + session.getQueryCount());
    }

    public void addLog(String text) {
        SwingUtilities.invokeLater(() -> {
            logArea.append(text + "\n");
            logArea.setCaretPosition(logArea.getDocument().getLength());
        });
    }

    public static void main(String[] args) {
        try { Class.forName("com.mysql.cj.jdbc.Driver"); } 
        catch (ClassNotFoundException e) { e.printStackTrace(); }
        SwingUtilities.invokeLater(() -> new Main().setVisible(true));
    } 
}