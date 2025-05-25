import math

class GraphProcessor:
    def __init__(self):
        self.size = 0
        self.original = []
        self._setup()

    def _setup(self):
        self.size = int(input("Введите количество вершин графа: "))
        print("Введите матрицу стоимостей (inf для бесконечности):")
        self.original = [
            [self._parse_value(x) for x in input().split()]
            for _ in range(self.size)
        ]

    @staticmethod
    def _parse_value(val):
        return math.inf if val.lower() == "inf" else int(val)

    def execute(self):
        route, cost = self._optimize_route(self.original)
        self._show_result(route, cost)

    def _optimize_route(self, matrix):
        path = []
        total = 0
        current = [row.copy() for row in matrix]
        
        for _ in range(self.size - 1):
            current, reduction = self._simplify_matrix(current)
            total += reduction
            
            zeros = self._find_zero_penalties(current)
            if not zeros: break
            
            selected = max(zeros, key=lambda x: x[2])
            path.append((selected[0], selected[1]))
            
            current = self._update_matrix(current, selected[0], selected[1])

        return self._finalize_path(path, total, matrix)

    def _simplify_matrix(self, m):
        cost = 0
        
        # Редукция строк
        for r in m:
            min_val = min(r)
            if min_val not in {0, math.inf}:
                cost += min_val
                r[:] = [x - min_val if x != math.inf else x for x in r]
        
        # Редукция столбцов
        for j in range(len(m)):
            col = [m[i][j] for i in range(len(m))]
            min_val = min(col)
            if min_val not in {0, math.inf}:
                cost += min_val
                for i in range(len(m)):
                    if m[i][j] != math.inf:
                        m[i][j] -= min_val
        
        return m, cost

    def _find_zero_penalties(self, m):
        penalties = []
        for i, row in enumerate(m):
            for j, val in enumerate(row):
                if val == 0:
                    row_vals = [x for k, x in enumerate(row) if k != j and x != math.inf]
                    col_vals = [m[k][j] for k in range(len(m)) if k != i and m[k][j] != math.inf]
                    penalty = (min(row_vals) if row_vals else 0) + (min(col_vals) if col_vals else 0)
                    penalties.append((i, j, penalty))
        return penalties

    def _update_matrix(self, m, i, j):
        new_m = [row.copy() for row in m]
        for row in new_m:
            row[j] = math.inf
        new_m[i] = [math.inf] * self.size
        new_m[j][i] = math.inf
        return new_m

    def _finalize_path(self, path, cost, original):
        if len(path) == self.size - 1:
            last = path[-1][1]
            missing = next(j for j in range(self.size) if j not in {x[0] for x in path})
            path.append((last, missing))
            cost += original[last][missing]
        return path, cost

    def _show_result(self, path, cost):
        print("\nОптимальный маршрут:")
        nodes = [str(x[0]+1) for x in path] + [str(path[-1][1]+1)]
        print(" → ".join(nodes))
        print(f"Суммарная стоимость: {cost}")

if __name__ == "__main__":
    GraphProcessor().execute()