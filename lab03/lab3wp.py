import tkinter as tk

class SimpleEditorApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Простой Undo (без паттерна)")
        self.root.geometry("650x400")

        # Текстовое поле
        self.text_widget = tk.Text(root, height=12, font=("Consolas", 12), bg="#f9f9f9")
        self.text_widget.pack(pady=10, padx=10, fill=tk.BOTH, expand=True)

        # Простой стек состояний
        self.history = [self.text_widget.get(1.0, tk.END)]
        self.last_text = self.history[0]  # Для проверки, изменился ли текст

        self.status_var = tk.StringVar(value="Готово. Состояний: 1")
        tk.Label(root, textvariable=self.status_var, bd=1, relief=tk.SUNKEN, anchor=tk.W, padx=5)\
          .pack(fill=tk.X, side=tk.BOTTOM)

        # Панель кнопок
        btn_frame = tk.Frame(root)
        btn_frame.pack(pady=10, padx=10, fill=tk.X)
        for i in range(3):
            btn_frame.grid_columnconfigure(i, weight=1)

        tk.Button(btn_frame, text="➕ Добавить", command=self.on_add, bg="#d1e7dd", 
                  width=20, height=2, font=('Arial', 11, 'bold')).grid(row=0, column=0, padx=5, sticky='ew')
        tk.Button(btn_frame, text="🗑️ Очистить", command=self.on_clear, bg="#f8d7da", 
                  width=20, height=2, font=('Arial', 11, 'bold')).grid(row=0, column=1, padx=5, sticky='ew')
        tk.Button(btn_frame, text="↩️ Undo", command=self.on_undo, bg="#fff3cd", 
                  width=20, height=2, font=('Arial', 11, 'bold')).grid(row=0, column=2, padx=5, sticky='ew')

        # Отслеживаем ручной ввод
        self.text_widget.bind('<KeyRelease>', self.on_key_release)

    def save_if_changed(self):
        """Сохраняет снимок текста, только если он реально изменился"""
        current = self.text_widget.get(1.0, tk.END)
        if current != self.last_text:
            self.history.append(current)
            self.last_text = current
            self.status_var.set(f"Изменено. Состояний: {len(self.history)}")

    def on_add(self):
        self.text_widget.insert(tk.END, "Hello World!\n")
        self.save_if_changed()

    def on_clear(self):
        if self.text_widget.get(1.0, tk.END).strip():
            self.text_widget.delete(1.0, tk.END)
            self.save_if_changed()

    def on_key_release(self, event):
        self.save_if_changed()

    def on_undo(self):
        if len(self.history) > 1:
            self.history.pop()          # Удаляем текущее состояние
            prev_state = self.history[-1]  # Берём предыдущее
            self.text_widget.delete(1.0, tk.END)
            self.text_widget.insert(1.0, prev_state)
            self.last_text = prev_state
            self.status_var.set(f"↩️ Отменено. Осталось: {len(self.history)}")
        else:
            self.status_var.set("⚠ Нечего отменять")

if __name__ == "__main__":
    root = tk.Tk()
    app = SimpleEditorApp(root)
    root.mainloop()