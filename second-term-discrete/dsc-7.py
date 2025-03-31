*волновой алгоритм
from collections import deque

class Graph:
    def __init__(self, vertices):
        self.V = vertices
        self.adj = {i: [] for i in range(vertices)}
    def add_edge(self, u, v):
        self.adj[u].append(v)
        self.adj[v].append(u) 
    def bfs(self, start, goal):
        visited = [False] * self.V
        parent = [-1] * self.V
        queue = deque([start])
        visited[start] = True
        while queue:
            current = queue.popleft()
            if current == goal:
                return self.reconstruct_path(parent, start, goal)
            for neighbor in self.adj[current]:
                if not visited[neighbor]:
                    visited[neighbor] = True
                    parent[neighbor] = current
                    queue.append(neighbor)
        return None
    def reconstruct_path(self, parent, start, goal):
        path = []
        current = goal
        while current != -1:
            path.append(current)
            current = parent[current]
        path.reverse()
        return path
def main():
    vertices = int(input("Введите количество вершин в графе: "))
    graph = Graph(vertices)
    while True:
        edge = input("Введите рёбра (в формате 'u v'), или 'done' для завершения: ")
        if edge.lower() == 'done':
            break
        u, v = map(int, edge.split())
        graph.add_edge(u, v)
    start = int(input("Введите стартовую вершину: "))
    goal = int(input("Введите целевую вершину: "))
    path = graph.bfs(start, goal)
    if path is not None:
        print("Кратчайший путь:", " -> ".join(map(str, path)))
    else:
        print("Путь не найден.")
if __name__ == '__main__':
    main()
