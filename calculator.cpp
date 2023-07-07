#include <QApplication>
#include <QGridLayout>
#include <QLineEdit>
#include <QPushButton>
#include <QWidget>
#include <QString>

int main(int argc, char *argv[]) {
    QApplication app(argc, argv);

    QWidget window;
    window.setWindowTitle("Kalkulator");

    QGridLayout *layout = new QGridLayout(&window);

    QLineEdit *display = new QLineEdit();
    layout->addWidget(display, 0, 0, 1, 4);

    QPushButton *buttons[10];
    for (int i = 0; i < 10; ++i) {
        buttons[i] = new QPushButton(QString::number(i));
    }

    layout->addWidget(buttons[7], 1, 0);
    layout->addWidget(buttons[8], 1, 1);
    layout->addWidget(buttons[9], 1, 2);
    layout->addWidget(buttons[4], 2, 0);
    layout->addWidget(buttons[5], 2, 1);
    layout->addWidget(buttons[6], 2, 2);
    layout->addWidget(buttons[1], 3, 0);
    layout->addWidget(buttons[2], 3, 1);
    layout->addWidget(buttons[3], 3, 2);
    layout->addWidget(buttons[0], 4, 0, 1, 3);

    QPushButton *addButton = new QPushButton("+");
    layout->addWidget(addButton, 1, 3);
    QPushButton *subtractButton = new QPushButton("-");
    layout->addWidget(subtractButton, 2, 3);
    QPushButton *multiplyButton = new QPushButton("*");
    layout->addWidget(multiplyButton, 3, 3);
    QPushButton *divideButton = new QPushButton("/");
    layout->addWidget(divideButton, 4, 3);

    QPushButton *equalsButton = new QPushButton("=");
    layout->addWidget(equalsButton, 4, 2);

    window.setLayout(layout);

    // Zmienne pomocnicze
    QString currentNumber = "";
    QString storedNumber = "";
    QString operation = "";

    // Sloty przycisków
    QObject::connect(addButton, &QPushButton::clicked, [&]() {
        storedNumber = display->text();
        operation = "+";
        display->clear();
    });

    QObject::connect(subtractButton, &QPushButton::clicked, [&]() {
        storedNumber = display->text();
        operation = "-";
        display->clear();
    });

    QObject::connect(multiplyButton, &QPushButton::clicked, [&]() {
        storedNumber = display->text();
        operation = "*";
        display->clear();
    });

    QObject::connect(divideButton, &QPushButton::clicked, [&]() {
        storedNumber = display->text();
        operation = "/";
        display->clear();
    });

    QObject::connect(equalsButton, &QPushButton::clicked, [&]() {
        QString result;
        double num1 = storedNumber.toDouble();
        double num2 = display->text().toDouble();

        if (operation == "+") {
            result = QString::number(num1 + num2);
        } else if (operation == "-") {
            result = QString::number(num1 - num2);
        } else if (operation == "*") {
            result = QString::number(num1 * num2);
        } else if (operation == "/") {
            if (num2 != 0) {
                result = QString::number(num1 / num2);
            } else {
                result = "Error";
            }
        }

        display->setText(result);
    });

    for (int i = 0; i < 10; ++i) {
        QObject::connect(buttons[i], &QPushButton::clicked, [&, i]() {
            currentNumber += QString::number(i);
            display->setText(currentNumber);
        });
    }

    window.show();

    return app.exec();
}
