class Graph:
    def __init__(self, vertices):
        self.V = vertices  
        self.graph = []  

    def add_edge(self, u, v, w):
        self.graph.append((u, v, w))

    def bellman_ford(self, src):

        distances = [float('inf')] * self.V
        distances[src] = 0 

        for _ in range(self.V - 1):
            for u, v, w in self.graph:
                if distances[u] != float('inf') and distances[u] + w < distances[v]:
                    distances[v] = distances[u] + w

        for u, v, w in self.graph:
            if distances[u] != float('inf') and distances[u] + w < distances[v]:
                print("Граф содержит отрицательный цикл")
                return None

        return distances


if __name__ == "__main__":
    g = Graph(5)
    g.add_edge(0, 1, -1)
    g.add_edge(0, 2, 4)
    g.add_edge(1, 2, 3)
    g.add_edge(1, 3, 2)
    g.add_edge(1, 4, 2)
    g.add_edge(3, 2, 5)
    g.add_edge(3, 1, 1)
    g.add_edge(4, 3, -3)

    src = 0
    distances = g.bellman_ford(src)
    if distances is not None:
        print("Расстояния от источника {}: {}".format(src, distances))
