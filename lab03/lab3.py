import tkinter as tk
from abc import ABC, abstractmethod


# 1. ABSTRACT COMMAND 

class Command(ABC):
    @abstractmethod
    def execute(self):
        pass

    @abstractmethod
    def undo(self):
        pass



# 2. RECEIVER 

class TextEditor:  # Это наш Receiver
    def __init__(self, text_widget):
        self.widget = text_widget
        self.last_saved_state = ""

    def add_text(self, text):
        self.widget.insert(tk.END, text + "\n")

    def clear_text(self):
        self.widget.delete(1.0, tk.END)

    def get_text(self):
        return self.widget.get(1.0, tk.END)

    def set_text(self, text):
        self.widget.delete(1.0, tk.END)
        self.widget.insert(1.0, text)

    def check_for_manual_change(self):
        current = self.get_text()
        if current != self.last_saved_state:
            cmd = ManualEditCommand(self, self.last_saved_state, current)
            self.last_saved_state = current
            return cmd
        return None



# 3. CONCRETE COMMANDS (наследуют от Command, содержат Receiver)


class AddTextCommand(Command):  # ConcreteCommand
    def __init__(self, receiver: TextEditor, text: str):
        self.receiver = receiver  # ← вот эта связь как на диаграмме!
        self.text = text
        self.old_state = None

    def execute(self):
        self.old_state = self.receiver.get_text()
        self.receiver.add_text(self.text)

    def undo(self):
        if self.old_state is not None:
            self.receiver.set_text(self.old_state)


class ClearCommand(Command):  # ConcreteCommand
    def __init__(self, receiver: TextEditor):
        self.receiver = receiver
        self.old_state = None

    def execute(self):
        self.old_state = self.receiver.get_text()
        self.receiver.clear_text()

    def undo(self):
        if self.old_state is not None:
            self.receiver.set_text(self.old_state)


class ManualEditCommand(Command):  # ConcreteCommand
    def __init__(self, receiver: TextEditor, old_text: str, new_text: str):
        self.receiver = receiver
        self.old_text = old_text
        self.new_text = new_text

    def execute(self):
        pass  # уже выполнено пользователем

    def undo(self):
        self.receiver.set_text(self.old_text)



# 4. INVOKER  — управляет командами

class CommandManager:  # Это наш Invoker
    def __init__(self):
        self.history = []  # вместо одного command — стек для Undo

    def execute(self, command: Command):
        command.execute()
        self.history.append(command)

    def undo(self):
        if self.history:
            last_cmd = self.history.pop()
            last_cmd.undo()
            return True
        return False


# 5. CLIENT (как на диаграмме) — создаёт всё и запускает

class App:  # Это наш Client
    def __init__(self, root):
        self.root = root
        self.root.title("Паттерн Command: Undo (под диаграмму)")
        self.root.geometry("650x400")

        # Создаём GUI элементы
        self.editor_widget = tk.Text(root, height=12, font=("Consolas", 12), bg="#f9f9f9")
        self.editor_widget.pack(pady=10, padx=10, fill=tk.BOTH, expand=True)

        # === СОЗДАЁМ КОМПОНЕНТЫ ПАТТЕРНА ===
        self.receiver = TextEditor(self.editor_widget)       # Receiver
        self.invoker = CommandManager()                      # Invoker

        # Начальное состояние
        self.receiver.last_saved_state = self.receiver.get_text()

        # Привязка событий
        self.editor_widget.bind('<KeyRelease>', self.on_key_release)

        # Кнопки
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

        # Статус бар
        self.status_var = tk.StringVar(value="Готов к работе. История: 0 команд")
        tk.Label(root, textvariable=self.status_var, bd=1, relief=tk.SUNKEN, anchor=tk.W, padx=5)\
          .pack(fill=tk.X, side=tk.BOTTOM)

    # === МЕТОДЫ КЛИЕНТА (Client Operations) ===

    def on_add(self):
        # Client создаёт ConcreteCommand, передаёт ему Receiver
        command = AddTextCommand(self.receiver, "Hello World!")
        # Client передаёт команду в Invoker
        self.invoker.execute(command)
        self.receiver.last_saved_state = self.receiver.get_text()
        self.status_var.set(f"Добавлен текст. История: {len(self.invoker.history)} команд")

    def on_clear(self):
        if self.editor_widget.get(1.0, tk.END).strip():
            command = ClearCommand(self.receiver)
            self.invoker.execute(command)
            self.receiver.last_saved_state = self.receiver.get_text()
            self.status_var.set(f"Текст очищен. История: {len(self.invoker.history)} команд")
        else:
            self.status_var.set("Поле уже пустое! История: " + str(len(self.invoker.history)))

    def on_key_release(self, event):
        cmd = self.receiver.check_for_manual_change()
        if cmd:
            self.invoker.execute(cmd)
            self.status_var.set(f"✍️ Ручное изменение. История: {len(self.invoker.history)} команд")

    def on_undo(self):
        success = self.invoker.undo()
        if success:
            self.receiver.last_saved_state = self.receiver.get_text()
            self.status_var.set(f"✓ Отменено! Осталось: {len(self.invoker.history)} команд")
        else:
            self.status_var.set("⚠ Нечего отменять (история пуста)")


if __name__ == "__main__":
    root = tk.Tk()
    app = App(root)
    root.mainloop()