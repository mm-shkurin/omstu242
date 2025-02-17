n = int(input("Введите размерность матрицы n: "))

matrix = [[0] * n for _ in range(n)]
for i in range(n):
    for j in range(n):
        matrix[i][j] = int(input(f"Введите элемент {i+1},{j+1}: "))
visited = [False] * n
components = []
for i in range(n):
    if not visited[i]:
        component = []
        stack = [i]
        while stack:
            current_vertex = stack.pop()
            if not visited[current_vertex]:
                visited[current_vertex] = True
                component.append(current_vertex + 1)
                for j in range(n):
                    if matrix[current_vertex][j] == 1 and not visited[j]:
                        stack.append(j)
        components.append(component)
print(f"Количество компонент связности: {len(components)}")
for index, component in enumerate(components):
    print(f"Компонента {index + 1}: {component}")
