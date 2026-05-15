package com.example;

import javax.swing.*;
import java.awt.*;
import java.sql.*;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;


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
    // Мы НЕ показываем статус в toString(), чтобы пользователь не знал его заранее

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
        // Показываем только название и автора. Статус скрыт!
        return title + " (" + author + ")"; 
    }
}


// IDENTITY MAP + ЛОГИКА БИБЛИОТЕКИ


class LibrarySession {
    private static final String DB_URL = "jdbc:mysql://localhost:3306/ooap_test?useSSL=false&allowPublicKeyRetrieval=true";
    private static final String DB_USER = "root";
    private static final String DB_PASS = "katikkotik"; 

    //  Кэш (Identity Map)
    private final Map<Integer, Reader> readerCache = new HashMap<>();
    private final Map<Integer, Book> bookCache = new HashMap<>();

    //  Счётчики
    private int fromDb = 0;
    private int fromCache = 0;

    public Connection getConnection() throws SQLException {
        return DriverManager.getConnection(DB_URL, DB_USER, DB_PASS);
    }

    //  Загрузка списка книг ДЛЯ ВЫПАДАЮЩЕГО СПИСКА
    // Здесь мы загружаем минимальную информацию (только для отображения названия)
    public List<Book> getAllBooksForList() {
        List<Book> list = new ArrayList<>();
        String sql = "SELECT id, title, author FROM books"; 
        try (Connection conn = getConnection(); 
             Statement stmt = conn.createStatement();
             ResultSet rs = stmt.executeQuery(sql)) {
            while (rs.next()) {
                Book b = new Book(
                    rs.getInt("id"),
                    rs.getString("title"),
                    rs.getString("author")
                );
                list.add(b);
                // Кладем упрощенный объект в кэш, чтобы при детальной загрузке обновить его или использовать этот
                // Но для чистоты эксперимента лучше загружать полный объект отдельно
            }
        } catch (SQLException e) { e.printStackTrace(); }
        return list;
    }

    //  Получить ПОЛНУЮ информацию о книге по ID (с кэшированием)
    // Именно этот метод проверяет статус is_available
    public Book getBookDetails(int id) {
        if (bookCache.containsKey(id)) {
            fromCache++;
            log("✅ [КЭШ] Полные данные книги #" + id + " взяты из памяти");
            return bookCache.get(id);
        }
        
        fromDb++;
        log("⬇️ [БД] Загружаем полные данные книги #" + id + "...");
        
        String sql = "SELECT * FROM books WHERE id = ?";
        try (Connection conn = getConnection(); 
             PreparedStatement ps = conn.prepareStatement(sql)) {
            ps.setInt(1, id);
            ResultSet rs = ps.executeQuery();
            if (rs.next()) {
                Book b = new Book(
                    rs.getInt("id"),
                    rs.getString("title"),
                    rs.getString("author")
                );
                // Сохраняем в кэш
                bookCache.put(id, b);
                log("💾 [КЭШ] Книга #" + id + " сохранена в памяти");
                return b;
            }
        } catch (SQLException e) { e.printStackTrace(); }
        return null;
    }

    //  Получить читателя по ID (с кэшированием)
    public Reader getReader(int id) {
        if (readerCache.containsKey(id)) {
            fromCache++;
            log("✅ [КЭШ] Читатель #" + id + " взят из памяти");
            return readerCache.get(id);
        }
        
        fromDb++;
        log("⬇️ [БД] Загружаем читателя #" + id + "...");
        
        String sql = "SELECT * FROM readers WHERE id = ?";
        try (Connection conn = getConnection(); 
             PreparedStatement ps = conn.prepareStatement(sql)) {
            ps.setInt(1, id);
            ResultSet rs = ps.executeQuery();
            if (rs.next()) {
                Reader r = new Reader(
                    rs.getInt("id"),
                    rs.getString("full_name"),
                    rs.getString("phone")
                );
                readerCache.put(id, r);
                log("💾 [КЭШ] Читатель #" + id + " сохранён в памяти");
                return r;
            }
        } catch (SQLException e) { e.printStackTrace(); }
        return null;
    }

    //  Найти кто держит книгу (JOIN с таблицей loans)
    public Reader findHolder(int bookId) {
        // Сначала проверим, есть ли книга в кэше и свободна ли она (оптимизация)
        // Но для демонстрации Identity Map мы явно вызовем getReader внутри
        
        String sql = """
            SELECT reader_id FROM loans 
            WHERE book_id = ? AND return_date IS NULL
            """;
        try (Connection conn = getConnection(); 
             PreparedStatement ps = conn.prepareStatement(sql)) {
            ps.setInt(1, bookId);
            ResultSet rs = ps.executeQuery();
            if (rs.next()) {
                int readerId = rs.getInt("reader_id");
                // 🔁 ВАЖНО: Используем метод getReader, чтобы сработал Identity Map!
                return getReader(readerId);
            }
        } catch (SQLException e) { e.printStackTrace(); }
        return null;
    }

    //  Очистить кэш
    public void clearCache() {
        readerCache.clear();
        bookCache.clear();
        log("🗑️ Кэш очищен. Следующая проверка пойдет в БД.");
    }

    //  Сброс счётчиков
    public void resetCounters() { fromDb = 0; fromCache = 0; }
    public int getFromDb() { return fromDb; }
    public int getFromCache() { return fromCache; }

    private void log(String msg) {
        System.out.println(msg);
        if (Main.getInstance() != null) Main.getInstance().addLog(msg);
    }
}


// GUI

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
    setTitle("📚 Библиотека: Проверка доступности (Identity Map)");
    setSize(900, 650);  // Увеличил размер окна
    setDefaultCloseOperation(EXIT_ON_CLOSE);
    setLocationRelativeTo(null);

    // 🔹 Верхняя панель с кнопками
    JPanel topPanel = new JPanel();
    topPanel.setLayout(new BoxLayout(topPanel, BoxLayout.Y_AXIS));
    topPanel.setBorder(BorderFactory.createEmptyBorder(15, 15, 15, 15));
    
    // Строка с выбором книги
    JPanel selectPanel = new JPanel(new FlowLayout(FlowLayout.LEFT, 10, 5));
    selectPanel.add(new JLabel("📚 Выберите книгу для проверки:"));
    comboBook = new JComboBox<>();
    comboBook.setPreferredSize(new Dimension(450, 28));
    comboBook.setMaximumRowCount(10);
    selectPanel.add(comboBook);
    
    // Строка с кнопками
    JPanel buttonPanel = new JPanel(new FlowLayout(FlowLayout.LEFT, 10, 5));
    
    JButton btnCheck = new JButton("🔍 Проверить статус");
    btnCheck.setFont(new Font("Segoe UI", Font.BOLD, 12));
    btnCheck.setPreferredSize(new Dimension(180, 35));
    btnCheck.setMaximumSize(new Dimension(180, 35));
    btnCheck.addActionListener(e -> checkBook());
    
    JButton btnClear = new JButton("🗑️ Очистить кэш");
    btnClear.setFont(new Font("Segoe UI", Font.PLAIN, 12));
    btnClear.setPreferredSize(new Dimension(150, 35));
    btnClear.setMaximumSize(new Dimension(150, 35));
    btnClear.addActionListener(e -> {
        session.clearCache();
        session.resetCounters();
        updateCounters();
        resultArea.setText("");
        addLog("--- Кэш сброшен пользователем ---\n");
    });
    
    buttonPanel.add(btnCheck);
    buttonPanel.add(Box.createHorizontalStrut(10)); // Отступ между кнопками
    buttonPanel.add(btnClear);
    
    topPanel.add(selectPanel);
    topPanel.add(Box.createVerticalStrut(10)); // Отступ между строками
    topPanel.add(buttonPanel);

    // 🔹 Центральная часть
    JPanel centerPanel = new JPanel(new BorderLayout(10, 10));
    centerPanel.setBorder(BorderFactory.createEmptyBorder(0, 15, 10, 15));

    logArea = new JTextArea(12, 0);
    logArea.setEditable(false);
    logArea.setFont(new Font("Consolas", Font.PLAIN, 11));
    logArea.setBackground(new Color(248, 248, 248));
    JScrollPane scrollLog = new JScrollPane(logArea);
    scrollLog.setBorder(BorderFactory.createTitledBorder(
        BorderFactory.createLineBorder(new Color(200, 200, 200)),
        "🔧 Технический лог (Identity Map)",
        javax.swing.border.TitledBorder.LEFT,
        javax.swing.border.TitledBorder.TOP
    ));

    resultArea = new JTextArea(6, 0);
    resultArea.setEditable(false);
    resultArea.setFont(new Font("Segoe UI", Font.PLAIN, 13));
    resultArea.setBackground(new Color(255, 255, 255));
    resultArea.setLineWrap(true);
    resultArea.setWrapStyleWord(true);
    JScrollPane scrollResult = new JScrollPane(resultArea);
    scrollResult.setBorder(BorderFactory.createTitledBorder(
        BorderFactory.createLineBorder(new Color(200, 200, 200)),
        "📋 Результат проверки",
        javax.swing.border.TitledBorder.LEFT,
        javax.swing.border.TitledBorder.TOP
    ));

    centerPanel.add(scrollLog, BorderLayout.CENTER);
    centerPanel.add(scrollResult, BorderLayout.SOUTH);

    // 🔹 Нижняя панель
    JPanel bottomPanel = new JPanel(new FlowLayout(FlowLayout.LEFT, 15, 10));
    lblCounters = new JLabel("📊 Статистика: БД = 0 | Кэш = 0");
    lblCounters.setFont(new Font("Segoe UI", Font.BOLD, 12));
    lblCounters.setForeground(new Color(0, 100, 200));
    bottomPanel.add(lblCounters);

    // Добавляем всё в окно
    add(topPanel, BorderLayout.NORTH);
    add(centerPanel, BorderLayout.CENTER);
    add(bottomPanel, BorderLayout.SOUTH);
    
    // Принудительно обновляем интерфейс
    validate();
    repaint();
}

    private void loadBooks() {
        addLog("⏳ Загрузка списка книг...");
        // Загружаем только названия, статус скрыт
        for (Book b : session.getAllBooksForList()) {
            comboBook.addItem(b);
        }
        addLog("✅ Загружено " + comboBook.getItemCount() + " книг.\n");
        addLog("💡 Выберите любую книгу. Вы не знаете, доступна она или нет.\n");
    }

    private void checkBook() {
        Book selectedBook = (Book) comboBook.getSelectedItem();
        if (selectedBook == null) {
            JOptionPane.showMessageDialog(this, "Выберите книгу из списка");
            return;
        }

        session.resetCounters();
        resultArea.setText("");
        
        addLog("\n" + "═".repeat(60));
        addLog("🚀 НАЧАЛО ПРОВЕРКИ: \"" + selectedBook.getTitle() + "\"");
        addLog("═".repeat(60));

        // 1️⃣ Запрашиваем полные данные книги (включая статус is_available)
        addLog("Шаг 1. Запрос данных книги из репозитория...");
        Book fullBookData = session.getBookDetails(selectedBook.getId());
        
        if (fullBookData == null) {
            resultArea.setText("❌ Ошибка: книга не найдена в базе.");
            return;
        }

      
        
        // Проверяем, занята ли книга, пытаясь найти держателя
        addLog("Шаг 2. Проверка наличия активной выдачи (loans)...");
        Reader holder = session.findHolder(fullBookData.getId());

        if (holder == null) {
            // Если держателя нет, значит книга свободна
            resultArea.setForeground(Color.GREEN.darker());
            resultArea.setText(
                "✅ СТАТУС: ДОСТУПНА\n\n" +
                "Книгу \"" + fullBookData.getTitle() + "\" можно взять на абонементе."
            );
            addLog("→ Результат: Книга свободна.");
        } else {
            // Если держатель найден, книга занята
            resultArea.setForeground(Color.RED.darker());
            resultArea.setText(
                "🔴 СТАТУС: ЗАНЯТА\n\n" +
                "Книгу держит:\n" +
                "👤 " + holder.getName() + "\n" +
                "📞 " + holder.getPhone()
            );
            addLog("→ Результат: Книга занята читателем ID=" + holder.getId());
        }

        // 📊 Итоговая статистика
        addLog("\n" + "─".repeat(60));
        addLog("📊 СТАТИСТИКА ПАТТЕРНА IDENTITY MAP:");
        addLog("   • Обращений к базе данных (SQL): " + session.getFromDb());
        addLog("   • Получено из кэша (Memory):     " + session.getFromCache());
        
        if (session.getFromCache() > 0) {
            addLog("   💡 ЭКОНОМИЯ: " + session.getFromCache() + " запросов не пошли в БД благодаря кэшу!");
        } else {
            addLog("   ℹ️  Это был первый запрос, поэтому кэш пуст.");
        }
        addLog("═".repeat(60) + "\n");
        
        updateCounters();
    }

    private void updateCounters() {
        lblCounters.setText("📊 Статистика текущей операции: БД = " + session.getFromDb() + " | Кэш = " + session.getFromCache());
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